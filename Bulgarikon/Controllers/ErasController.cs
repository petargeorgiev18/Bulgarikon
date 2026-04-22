using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.EraDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.ViewModels.EraViewModels;
using Bulgarikon.ViewModels.ImageViewModels;
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
            var dtos = await eraService.GetAllAsync();

            var model = dtos.Select(e => new EraViewViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                Images = e.Images?.Select(i => new ImageViewViewModel
                {
                    Id = i.Id,
                    Url = i.Url,
                    Caption = i.Caption
                }).ToList()
            });

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var dto = await eraService.GetByIdAsync(id);

                var model = new EraViewViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    StartYear = dto.StartYear,
                    EndYear = dto.EndYear,
                    Images = dto.Images?.Select(i => new ImageViewViewModel
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption
                    }).ToList()
                };

                return View(model);
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
            return View(new EraFormViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EraFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new EraFormDto
            {
                Name = model.Name,
                Description = model.Description,
                StartYear = model.StartYear,
                EndYear = model.EndYear,
                Images = model.Images?.Select(i => new ImageEditDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    Caption = i.Caption,
                    Remove = i.Remove
                }).ToList(),
                ImageFiles = model.ImageFiles
            };

            await eraService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var dto = await eraService.GetByIdAsync(id);

                var model = new EraFormViewModel
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    StartYear = dto.StartYear,
                    EndYear = dto.EndYear,
                    Images = dto.Images?.Select(i => new ImageEditViewModel
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption,
                        Remove = false
                    }).ToList()
                };

                ViewBag.EraId = id;

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
        public async Task<IActionResult> Edit(Guid id, EraFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new EraFormDto
            {
                Name = model.Name,
                Description = model.Description,
                StartYear = model.StartYear,
                EndYear = model.EndYear,
                Images = model.Images?.Select(i => new ImageEditDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    Caption = i.Caption,
                    Remove = i.Remove
                }).ToList(),
                ImageFiles = model.ImageFiles
            };

            await eraService.EditAsync(id, dto);

            return RedirectToAction(nameof(Details), new { id });
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

                var dto = await eraService.GetByIdAsync(id);

                var model = new EraViewViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    StartYear = dto.StartYear,
                    EndYear = dto.EndYear
                };

                return View("Delete", model);
            }
        }
    }
}