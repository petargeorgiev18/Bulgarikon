using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.FigureDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.ViewModels.FigureViewModels;
using Bulgarikon.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Controllers
{
    public class FiguresController : Controller
    {
        private readonly IFigureService figuresService;
        private readonly IEraService erasService;
        private readonly ICivilizationService civilizationsService;

        public FiguresController(
            IFigureService figuresService,
            IEraService erasService,
            ICivilizationService civilizationsService)
        {
            this.figuresService = figuresService;
            this.erasService = erasService;
            this.civilizationsService = civilizationsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? eraId, Guid? civilizationId)
        {
            await LoadDropdownsAsync(eraId, civilizationId);

            var dtos = await figuresService.GetByEraAsync(eraId, civilizationId);

            var model = dtos.Select(x => new FigureViewViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                BirthDate = x.BirthDate,
                DeathDate = x.DeathDate,
                BirthYear = x.BirthYear,
                DeathYear = x.DeathYear,
                EraId = x.EraId,
                EraName = x.EraName,
                CivilizationId = x.CivilizationId,
                CivilizationName = x.CivilizationName,
                Images = x.Images.Select(i => new ImageViewViewModel
                {
                    Id = i.Id,
                    Url = i.Url,
                    Caption = i.Caption
                }).ToList()
            }).ToList();

            ViewBag.EraId = eraId;
            ViewBag.CivilizationId = civilizationId;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dto = await figuresService.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var model = new FigureViewViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                BirthDate = dto.BirthDate,
                DeathDate = dto.DeathDate,
                BirthYear = dto.BirthYear,
                DeathYear = dto.DeathYear,
                EraId = dto.EraId,
                EraName = dto.EraName,
                CivilizationId = dto.CivilizationId,
                CivilizationName = dto.CivilizationName,
                Images = dto.Images.Select(i => new ImageViewViewModel
                {
                    Id = i.Id,
                    Url = i.Url,
                    Caption = i.Caption
                }).ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(Guid eraId, Guid? civilizationId)
        {
            await LoadDropdownsAsync(eraId, civilizationId);

            return View(new FigureFormViewModel
            {
                EraId = eraId,
                CivilizationId = civilizationId
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FigureFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                return View(model);
            }

            try
            {
                var dto = new FigureFormDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    BirthDate = model.BirthDate,
                    DeathDate = model.DeathDate,
                    BirthYear = model.BirthYear,
                    DeathYear = model.DeathYear,
                    EraId = model.EraId,
                    CivilizationId = model.CivilizationId,
                    Images = model.Images.Select(i => new ImageEditDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption,
                        Remove = i.Remove
                    }).ToList(),
                    ImageFiles = model.ImageFiles
                };

                var id = await figuresService.CreateAsync(dto);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dto = await figuresService.GetForEditAsync(id);
            if (dto == null) return NotFound();

            await LoadDropdownsAsync(dto.EraId, dto.CivilizationId);
            ViewBag.FigureId = id;

            var model = new FigureFormViewModel
            {
                Name = dto.Name,
                Description = dto.Description,
                BirthDate = dto.BirthDate,
                DeathDate = dto.DeathDate,
                BirthYear = dto.BirthYear,
                DeathYear = dto.DeathYear,
                EraId = dto.EraId,
                CivilizationId = dto.CivilizationId,
                Images = dto.Images.Select(i => new ImageEditViewModel
                {
                    Id = i.Id,
                    Url = i.Url,
                    Caption = i.Caption,
                    Remove = i.Remove
                }).ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FigureFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                ViewBag.FigureId = id;
                return View(model);
            }

            try
            {
                var dto = new FigureFormDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    BirthDate = model.BirthDate,
                    DeathDate = model.DeathDate,
                    BirthYear = model.BirthYear,
                    DeathYear = model.DeathYear,
                    EraId = model.EraId,
                    CivilizationId = model.CivilizationId,
                    Images = model.Images.Select(i => new ImageEditDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption,
                        Remove = i.Remove
                    }).ToList(),
                    ImageFiles = model.ImageFiles
                };

                await figuresService.UpdateAsync(id, dto);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                ViewBag.FigureId = id;
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, Guid eraId, Guid? civilizationId)
        {
            await figuresService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { eraId, civilizationId });
        }

        private async Task LoadDropdownsAsync(Guid? eraId, Guid? civilizationId)
        {
            var eras = await erasService.GetAllAsync();
            ViewBag.Eras = eras.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = e.Id == eraId
            }).ToList();

            var civs = await civilizationsService.GetByEraAsync(eraId);
            ViewBag.Civilizations = civs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = civilizationId.HasValue && c.Id == civilizationId.Value
            }).ToList();
        }
    }
}