using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.FigureDTOs;
using Bulgarikon.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class FiguresController : Controller
{
    private readonly IFigureService figuresService;
    private readonly IEraService erasService;
    private readonly ICivilizationService civilizationsService;

    public FiguresController(IFigureService figuresService, IEraService erasService,
        ICivilizationService civilizationsService)
    {
        this.figuresService = figuresService;
        this.erasService = erasService;
        this.civilizationsService = civilizationsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid eraId, Guid? civilizationId)
    {
        await LoadDropdownsAsync(eraId, civilizationId);

        var model = await figuresService.GetByEraAsync(eraId, civilizationId);
        ViewBag.EraId = eraId;
        ViewBag.CivilizationId = civilizationId;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        FigureViewDto? model = await figuresService.GetDetailsAsync(id);
        if (model == null) 
            return NotFound();
        return View(model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create(Guid eraId, Guid? civilizationId)
    {
        await LoadDropdownsAsync(eraId, civilizationId);

        return View(new FigureFormDto
        {
            EraId = eraId,
            CivilizationId = civilizationId
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FigureFormDto model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(model.EraId, model.CivilizationId);
            return View(model);
        }

        Guid id = await figuresService.CreateAsync(model);
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await figuresService.GetForEditAsync(id);
        if (model == null) return NotFound();

        await LoadDropdownsAsync(model.EraId, model.CivilizationId);
        ViewBag.FigureId = id;

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, FigureFormDto model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(model.EraId, model.CivilizationId);
            ViewBag.FigureId = id;
            return View(model);
        }

        await figuresService.UpdateAsync(id, model);
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, Guid eraId, Guid? civilizationId)
    {
        await figuresService.DeleteAsync(id);
        return RedirectToAction(nameof(Index), new { eraId, civilizationId });
    }

    // Helper method to load dropdown data
    private async Task LoadDropdownsAsync(Guid eraId, Guid? civilizationId)
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