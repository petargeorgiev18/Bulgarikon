using Bulgarikon.Data;
using Bulgarikon.Models;
using Bulgarikon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bulgarikon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BulgarikonDbContext context;

        public HomeController(ILogger<HomeController> logger, BulgarikonDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeStatsViewModel
            {
                ErasCount = await context.Eras.CountAsync(),
                EventsCount = await context.Events.CountAsync(),
                FiguresCount = await context.Figures.CountAsync(),
                ArtifactsCount = await context.Artifacts.CountAsync()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
