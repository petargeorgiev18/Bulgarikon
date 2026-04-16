using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.ViewModels.CivilizaionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulgarikon.Controllers
{
    public class CivilizationsController : Controller
    {
        private readonly ICivilizationService civilizationsService;
        private readonly IEraService erasService;

        public CivilizationsController(
            ICivilizationService civilizationsService,
            IEraService erasService)
        {
            this.civilizationsService = civilizationsService;
            this.erasService = erasService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid eraId)
        {
            var dtos = await civilizationsService.GetByEraAsync(eraId);

            var model = dtos.Select(c => new CivilizationViewViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
                EraId = c.EraId,
                EraName = c.EraName
            }).ToList();

            ViewBag.EraId = eraId;
            await LoadDropdownsAsync(eraId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dto = await civilizationsService.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var model = new CivilizationViewViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type,
                EraId = dto.EraId,
                EraName = dto.EraName
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(Guid eraId)
        {
            await LoadDropdownsAsync(eraId);

            return View(new CivilizationFormViewModel
            {
                EraId = eraId
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilizationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId);
                return View(model);
            }

            var dto = new CivilizationFormDto
            {
                Name = model.Name,
                Description = model.Description,
                Type = model.Type,
                EraId = model.EraId
            };

            var id = await civilizationsService.CreateAsync(dto);

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dto = await civilizationsService.GetForEditAsync(id);
            if (dto == null) return NotFound();

            await LoadDropdownsAsync(dto.EraId);

            return View(new CivilizationFormViewModel
            {
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type,
                EraId = dto.EraId
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CivilizationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId);
                return View(model);
            }

            var dto = new CivilizationFormDto
            {
                Name = model.Name,
                Description = model.Description,
                Type = model.Type,
                EraId = model.EraId
            };

            await civilizationsService.UpdateAsync(id, dto);

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid eraId)
        {
            await civilizationsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { eraId });
        }

        private async Task LoadDropdownsAsync(Guid? selectedEraId = null)
        {
            ViewBag.CivilizationTypes = Enum
                .GetValues<CivilizationType>()
                .Select(t => new SelectListItem
                {
                    Text = t.ToString(),
                    Value = t.ToString()
                })
                .ToList();

            var eras = await erasService.GetAllAsync();

            ViewBag.Eras = eras.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = selectedEraId.HasValue && e.Id == selectedEraId.Value
            }).ToList();
        }
    }
}