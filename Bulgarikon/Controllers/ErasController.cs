using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.EraDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulgarikon.Controllers
{
    public class ErasController : Controller
    {
        private readonly IEraService eraService;
        public ErasController(IEraService eraService)
        {
            this.eraService = eraService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<EraViewDto> eras = await eraService.GetAllAsync();
            return View(eras);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var era = await eraService.GetByIdAsync(id);
                return View(era);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EraFormDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await eraService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var era = await eraService.GetByIdAsync(id);

                ViewBag.EraId = id;

                var model = new EraFormDto
                {
                    Name = era.Name,
                    Description = era.Description ?? string.Empty,
                    StartYear = era.StartYear,
                    EndYear = era.EndYear
                };

                return View(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EraFormDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await eraService.EditAsync(id, model);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await eraService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var era = await eraService.GetByIdAsync(id);
                return View("Delete", era);
            }
        }
    }
}
