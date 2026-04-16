using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using Bulgarikon.Core.DTOs.EventDTOs;
using Bulgarikon.Core.DTOs.FigureDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Models;
using Bulgarikon.ViewModels.EventViewModels;
using Bulgarikon.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService eventsService;
        private readonly IEraService erasService;
        private readonly ICivilizationService civilizationsService;
        private readonly IFigureService figuresService;

        public EventsController(
            IEventService eventsService,
            IEraService erasService,
            ICivilizationService civilizationsService,
            IFigureService figuresService)
        {
            this.eventsService = eventsService;
            this.erasService = erasService;
            this.civilizationsService = civilizationsService;
            this.figuresService = figuresService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? eraId, string? search)
        {
            search = (search ?? string.Empty).Trim();

            var eras = (await erasService.GetAllAsync())
                .OrderBy(e => e.StartYear)
                .ToList();

            if (eraId.HasValue && eraId.Value != Guid.Empty)
                eras = eras.Where(e => e.Id == eraId.Value).ToList();

            var groups = new List<EventEraGroupViewModel>();

            foreach (var era in eras)
            {
                var dtos = await eventsService.GetByEraAsync(era.Id);

                var events = dtos.Select(e => new EventViewViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear,
                    Location = e.Location,
                    EraId = e.EraId,
                    EraName = e.EraName
                }).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.ToLowerInvariant();

                    events = events.Where(ev =>
                        ev.Title.ToLowerInvariant().Contains(s) ||
                        (ev.Location ?? "").ToLowerInvariant().Contains(s) ||
                        ev.StartYear.ToString().Contains(s) ||
                        ev.EndYear.ToString().Contains(s)
                    ).ToList();
                }

                groups.Add(new EventEraGroupViewModel
                {
                    EraId = era.Id,
                    EraName = era.Name,
                    EraStartYear = era.StartYear,
                    EraEndYear = era.EndYear,
                    Events = events
                });
            }

            if (!string.IsNullOrWhiteSpace(search))
                groups = groups.Where(g => g.Events.Any()).ToList();

            ViewBag.Eras = (await erasService.GetAllAsync())
                .OrderBy(e => e.StartYear)
                .Select(e => new SelectListItem
                {
                    Text = $"{e.Name} ({e.StartYear}–{e.EndYear})",
                    Value = e.Id.ToString(),
                    Selected = eraId.HasValue && e.Id == eraId.Value
                })
                .ToList();

            ViewBag.EraId = eraId;
            ViewBag.Search = search;

            return View(groups);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dto = await eventsService.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            await LoadDropdownsAsync(dto.EraId);

            var model = new EventDetailsViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                StartYear = dto.StartYear,
                EndYear = dto.EndYear,
                EraId = dto.EraId,
                EraName = dto.EraName,

                Images = dto.Images.Select(i => new ImageViewViewModel
                {
                    Id = i.Id,
                    Url = i.Url,
                    Caption = i.Caption
                }).ToList(),

                Civilizations = dto.Civilizations.Select(c => new CivilizationChipViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),

                Figures = dto.Figures.Select(f => new FigureChipViewModel
                {
                    Id = f.Id,
                    Name = f.Name
                }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? eraId = null)
        {
            await LoadDropdownsAsync(eraId);

            return View(new EventFormViewModel
            {
                EraId = eraId ?? Guid.Empty,
                CivilizationIds = new List<Guid>(),
                FigureIds = new List<Guid>()
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId);
                return View(model);
            }

            try
            {
                var dto = new EventFormDto
                {
                    Title = model.Title,
                    Description = model.Description,
                    Location = model.Location,
                    StartYear = model.StartYear,
                    EndYear = model.EndYear,
                    EraId = model.EraId,
                    CivilizationIds = model.CivilizationIds,
                    FigureIds = model.FigureIds
                };

                var id = await eventsService.CreateAsync(dto);

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException ex)
            {
                AddYearErrorsToModelState(model, ex.Message);
                await LoadDropdownsAsync(model.EraId);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                AddYearErrorsToModelState(model, ex.Message);
                await LoadDropdownsAsync(model.EraId);
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dto = await eventsService.GetForEditAsync(id);
            if (dto == null) return NotFound();

            await LoadDropdownsAsync(dto.EraId);

            var model = new EventFormViewModel
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                StartYear = dto.StartYear,
                EndYear = dto.EndYear,
                EraId = dto.EraId,
                CivilizationIds = dto.CivilizationIds,
                FigureIds = dto.FigureIds,
                ImageFiles = dto.ImageFiles,
                Images = dto.Images?.Select(x => new ImageEditViewModel
                {
                    Id = x.Id,
                    Url = x.Url,
                    Caption = x.Caption,
                    Remove = x.Remove
                }).ToList()
            };

            ViewBag.EventId = id;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId);
                ViewBag.EventId = id;
                return View(model);
            }

            try
            {
                var dto = new EventFormDto
                {
                    Title = model.Title,
                    Description = model.Description,
                    Location = model.Location,
                    StartYear = model.StartYear,
                    EndYear = model.EndYear,
                    EraId = model.EraId,
                    CivilizationIds = model.CivilizationIds,
                    FigureIds = model.FigureIds,
                    ImageFiles = model.ImageFiles,
                    Images = model.Images?.Select(x => new ImageEditDto
                    {
                        Id = x.Id,
                        Url = x.Url,
                        Caption = x.Caption,
                        Remove = x.Remove
                    }).ToList()
                };

                await eventsService.UpdateAsync(id, dto);

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException ex)
            {
                AddYearErrorsToModelState(model, ex.Message);
                await LoadDropdownsAsync(model.EraId);
                ViewBag.EventId = id;
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                AddYearErrorsToModelState(model, ex.Message);
                await LoadDropdownsAsync(model.EraId);
                ViewBag.EventId = id;
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await eventsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync(Guid? selectedEraId = null)
        {
            var eras = await erasService.GetAllAsync();

            ViewBag.Eras = eras.Select(e => new SelectListItem
            {
                Text = $"{e.Name} ({e.StartYear}–{e.EndYear})",
                Value = e.Id.ToString(),
                Selected = selectedEraId == e.Id
            }).ToList();

            var civs = selectedEraId.HasValue
                ? await civilizationsService.GetByEraAsync(selectedEraId.Value)
                : Enumerable.Empty<CivilizationViewDto>();

            ViewBag.Civilizations = civs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            var figs = selectedEraId.HasValue
                ? await figuresService.GetByEraAsync(selectedEraId.Value, null)
                : Enumerable.Empty<FigureViewDto>();

            ViewBag.Figures = figs.Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.Id.ToString()
            }).ToList();
        }

        private void AddYearErrorsToModelState(EventFormViewModel model, string message)
        {
            ModelState.AddModelError(string.Empty, message);
            ModelState.AddModelError(nameof(model.StartYear), message);
            ModelState.AddModelError(nameof(model.EndYear), message);
        }
    }
}