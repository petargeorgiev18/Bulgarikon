using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class CivilizationsController : Controller
{
    private readonly ICivilizationService civilizationsService;
    private readonly IEraService erasService;

    public CivilizationsController(ICivilizationService civilizationsservice, IEraService erasservice)
    {
        this.civilizationsService = civilizationsservice;
        this.erasService = erasservice;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid eraId)
    {
        var model = await civilizationsService.GetByEraAsync(eraId);
        ViewBag.EraId = eraId;
        await LoadDropdownsAsync(selectedEraId: eraId);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var model = await civilizationsService.GetDetailsAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create(Guid eraId)
    {
        await LoadDropdownsAsync(selectedEraId: eraId);

        var model = new CivilizationFormDto
        {
            EraId = eraId
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CivilizationFormDto model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(model.EraId);
            return View(model);
        }

        var id = await civilizationsService.CreateAsync(model);
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await civilizationsService.GetForEditAsync(id);
        if (model == null) return NotFound();

        await LoadDropdownsAsync(model.EraId);
        ViewBag.CivilizationId = id;

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CivilizationFormDto model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(model.EraId);
            ViewBag.CivilizationId = id;
            return View(model);
        }

        await civilizationsService.UpdateAsync(id, model);
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

    //Helper method to load dropdown data
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

        var eraList = await erasService.GetAllAsync();
        ViewBag.Eras = eraList.Select(e => new SelectListItem
        {
            Text = e.Name,
            Value = e.Id.ToString(),
            Selected = selectedEraId.HasValue && e.Id == selectedEraId.Value
        }).ToList();
    }
}