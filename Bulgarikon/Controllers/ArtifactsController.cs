using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.ArtifactDTOs;
using Bulgarikon.Core.Interfaces;
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
            "Злато", "Сребро", "Бронз", "Желязо",
            "Камък", "Керамика", "Дърво", "Кожа",
            "Текстил", "Стъкло", "Кост", "Пергамент",
            "Монета", "Друг"
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

            var model = await artifactsService.GetByEraAsync(eraId, civilizationId);

            ViewBag.EraId = eraId;
            ViewBag.CivilizationId = civilizationId;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await artifactsService.GetDetailsAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? eraId, Guid? civilizationId)
        {
            var eras = (await erasService.GetAllAsync()).ToList();
            var defaultEraId = eraId ?? eras.FirstOrDefault()?.Id;

            if (defaultEraId == null)
                return BadRequest("Няма налични епохи. Добави епоха преди да създаваш артефакт.");

            await LoadDropdownsAsync(defaultEraId, civilizationId);

            return View(new ArtifactFormDto
            {
                EraId = defaultEraId.Value,
                CivilizationId = civilizationId,
                DiscoveredAt = DateTime.Today
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtifactFormDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                return View(model);
            }

            var id = await artifactsService.CreateAsync(model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await artifactsService.GetForEditAsync(id);
            if (model == null) return NotFound();

            await LoadDropdownsAsync(model.EraId, model.CivilizationId);
            ViewBag.ArtifactId = id;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ArtifactFormDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.EraId, model.CivilizationId);
                ViewBag.ArtifactId = id;
                return View(model);
            }

            await artifactsService.UpdateAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid? eraId, Guid? civilizationId)
        {
            await artifactsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { eraId, civilizationId });
        }

        private async Task LoadDropdownsAsync(Guid? eraId, Guid? civilizationId)
        {
            var eras = await erasService.GetAllAsync();
            ViewBag.Eras = eras.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = eraId.HasValue && e.Id == eraId.Value
            }).ToList();

            var civs = await civilizationsService.GetByEraAsync(eraId);
            var civItems = civs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = civilizationId.HasValue && c.Id == civilizationId.Value
            }).ToList();

            civItems.Insert(0, new SelectListItem { Text = "(няма)", Value = "" });
            ViewBag.Civilizations = civItems;

            ViewBag.Materials = MaterialOptions.Select(m => new SelectListItem
            {
                Text = m,
                Value = m
            }).ToList();
        }
    }
}