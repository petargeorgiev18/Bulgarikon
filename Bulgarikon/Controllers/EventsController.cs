using System.ComponentModel.DataAnnotations;
using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using Bulgarikon.Core.DTOs.EventDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                var events = (await eventsService.GetByEraAsync(era.Id)).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.ToLowerInvariant();

                    events = events
                        .Where(ev =>
                            (!string.IsNullOrWhiteSpace(ev.Title) && ev.Title.ToLowerInvariant().Contains(s)) ||
                            (!string.IsNullOrWhiteSpace(ev.Location) && ev.Location.ToLowerInvariant().Contains(s)) ||
                            ev.StartYear.ToString().Contains(s) ||
                            ev.EndYear.ToString().Contains(s))
                        .ToList();
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

            ViewBag.EraId = eraId;
            ViewBag.Search = search;

            ViewBag.Eras = (await erasService.GetAllAsync())
                .OrderBy(e => e.StartYear)
                .Select(e => new SelectListItem
                {
                    Text = $"{e.Name} ({e.StartYear}–{e.EndYear})",
                    Value = e.Id.ToString(),
                    Selected = eraId.HasValue && eraId.Value != Guid.Empty && e.Id == eraId.Value
                })
                .ToList();

            return View(groups);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await eventsService.GetDetailsAsync(id);
            if (model == null) return NotFound();

            await LoadDropdownsAsync(model.EraId);

            var civItems = (List<SelectListItem>)ViewBag.Civilizations;
            var figItems = (List<SelectListItem>)ViewBag.Figures;

            var selectedCivIds = model.Civilizations.Select(c => c.Id.ToString()).ToHashSet();
            var selectedFigIds = model.Figures.Select(f => f.Id.ToString()).ToHashSet();

            ViewBag.Civilizations = civItems.Where(x => !selectedCivIds.Contains(x.Value!)).ToList();
            ViewBag.Figures = figItems.Where(x => !selectedFigIds.Contains(x.Value!)).ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? eraId = null)
        {
            await LoadDropdownsAsync(selectedEraId: eraId);

            var model = new EventFormDto
            {
                EraId = eraId ?? Guid.Empty,
                CivilizationIds = new List<Guid>(),
                FigureIds = new List<Guid>()
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventFormDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId);
                return View(model);
            }

            try
            {
                var id = await eventsService.CreateAsync(model);
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
            var model = await eventsService.GetForEditAsync(id);
            if (model == null) return NotFound();

            await LoadDropdownsAsync(model.EraId);
            ViewBag.EventId = id;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EventFormDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId);
                ViewBag.EventId = id;
                return View(model);
            }

            try
            {
                await eventsService.UpdateAsync(id, model);
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCivilization(Guid eventId, Guid civilizationId)
        {
            await eventsService.AddCivilizationAsync(eventId, civilizationId);
            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFigure(Guid eventId, Guid figureId)
        {
            await eventsService.AddFigureAsync(eventId, figureId);
            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        private async Task LoadDropdownsAsync(Guid? selectedEraId = null)
        {
            var eras = await erasService.GetAllAsync();
            ViewBag.Eras = eras
                .OrderBy(e => e.StartYear)
                .Select(e => new SelectListItem
                {
                    Text = $"{e.Name} ({e.StartYear}–{e.EndYear})",
                    Value = e.Id.ToString(),
                    Selected = selectedEraId.HasValue && e.Id == selectedEraId.Value
                })
                .ToList();

            var civs = selectedEraId.HasValue && selectedEraId.Value != Guid.Empty
                ? await civilizationsService.GetByEraAsync(selectedEraId.Value)
                : Enumerable.Empty<CivilizationViewDto>();

            ViewBag.Civilizations = civs
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToList();

            var figs = selectedEraId.HasValue && selectedEraId.Value != Guid.Empty
                ? await figuresService.GetByEraAsync(selectedEraId.Value)
                : Enumerable.Empty<Bulgarikon.Core.DTOs.FigureDTOs.FigureViewDto>();

            ViewBag.Figures = figs
                .OrderBy(f => f.Name)
                .Select(f => new SelectListItem { Text = f.Name, Value = f.Id.ToString() })
                .ToList();
        }

        private void AddYearErrorsToModelState(EventFormDto model, string message)
        {
            ModelState.AddModelError(string.Empty, message);
            ModelState.AddModelError(nameof(model.StartYear), message);
            ModelState.AddModelError(nameof(model.EndYear), message);
        }
    }
}