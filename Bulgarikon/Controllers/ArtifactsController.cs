using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.ArtifactDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.ViewModels.ArtifactViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulgarikon.Controllers
{
    public class ArtifactsController : Controller
    {
        private readonly IArtifactService artifactsService;
        private readonly IEraService erasService;
        private readonly ICivilizationService civilizationsService;

        private static readonly string[] MaterialOptions =
        {
            "Злато","Сребро","Бронз","Желязо","Камък","Керамика",
            "Дърво","Кожа","Текстил","Стъкло","Кост","Пергамент","Монета","Друг"
        };

        public ArtifactsController(
            IArtifactService artifactsService,
            IEraService erasService,
            ICivilizationService civilizationsService)
        {
            this.artifactsService = artifactsService;
            this.erasService = erasService;
            this.civilizationsService = civilizationsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? eraId, Guid? civilizationId)
        {
            await LoadDropdownsAsync(eraId, civilizationId);

            var dtos = await artifactsService.GetByEraAsync(eraId, civilizationId);

            var model = dtos.Select(a => new ArtifactViewViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Year = a.Year,
                Material = a.Material,
                Location = a.Location,
                EraId = a.EraId,
                EraName = a.EraName,
                CivilizationId = a.CivilizationId,
                CivilizationName = a.CivilizationName,
                ImageUrl = a.ImageUrl
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dto = await artifactsService.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var model = new ArtifactDetailsViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Material = dto.Material,
                Location = dto.Location,
                Year = dto.Year,
                DiscoveredAt = dto.DiscoveredAt,
                EraId = dto.EraId,
                EraName = dto.EraName,
                CivilizationId = dto.CivilizationId,
                CivilizationName = dto.CivilizationName,
                ImageUrl = dto.ImageUrl
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? eraId, Guid? civilizationId)
        {
            var eras = (await erasService.GetAllAsync()).ToList();
            var defaultEraId = eraId ?? eras.FirstOrDefault()?.Id;

            if (defaultEraId == null)
                return BadRequest("No eras.");

            await LoadDropdownsAsync(defaultEraId, civilizationId);

            return View(new ArtifactFormViewModel
            {
                EraId = defaultEraId.Value,
                CivilizationId = civilizationId,
                DiscoveredAt = DateTime.Today
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtifactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                return View(model);
            }

            var id = await artifactsService.CreateAsync(new ArtifactFormDto
            {
                Name = model.Name,
                Description = model.Description,
                Year = model.Year,
                Material = model.Material,
                Location = model.Location,
                DiscoveredAt = model.DiscoveredAt,
                EraId = model.EraId,
                CivilizationId = model.CivilizationId,
                ImageUrl = model.ImageUrl
            });

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dto = await artifactsService.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            await LoadDropdownsAsync(dto.EraId, dto.CivilizationId);

            var model = new ArtifactFormViewModel
            {
                Name = dto.Name,
                Description = dto.Description,
                Material = dto.Material,
                Location = dto.Location,
                Year = dto.Year,
                DiscoveredAt = dto.DiscoveredAt,
                EraId = dto.EraId,
                CivilizationId = dto.CivilizationId,
                ImageUrl = dto.ImageUrl
            };

            ViewBag.ArtifactId = id;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ArtifactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                ViewBag.ArtifactId = id;
                return View(model);
            }

            await artifactsService.UpdateAsync(id, new ArtifactFormDto
            {
                Name = model.Name,
                Description = model.Description,
                Material = model.Material,
                Location = model.Location,
                Year = model.Year,
                DiscoveredAt = model.DiscoveredAt,
                EraId = model.EraId,
                CivilizationId = model.CivilizationId,
                ImageUrl = model.ImageUrl,
                ImageFile = model.ImageFile
            });

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await artifactsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync(Guid? eraId, Guid? civId)
        {
            var eras = await erasService.GetAllAsync();
            ViewBag.Eras = eras.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = eraId == e.Id
            }).ToList();

            var civs = await civilizationsService.GetByEraAsync(eraId);
            ViewBag.Civilizations = civs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = civId == c.Id
            }).ToList();

            ViewBag.Materials = MaterialOptions.Select(m => new SelectListItem
            {
                Text = m,
                Value = m
            }).ToList();
        }
    }
}