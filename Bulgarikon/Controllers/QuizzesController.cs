using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.QuizDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulgarikon.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly IQuizService quizzes;
        private readonly IQuizResultService results;
        private readonly IEraService eras;
        private readonly UserManager<BulgarikonUser> userManager;

        public QuizzesController(
            IQuizService quizzes,
            IQuizResultService results,
            IEraService eras,
            UserManager<BulgarikonUser> userManager)
        {
            this.quizzes = quizzes;
            this.results = results;
            this.eras = eras;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? eraId)
        {
            await LoadErasAsync(eraId);
            var model = await quizzes.GetByEraAsync(eraId);
            ViewBag.EraId = eraId;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await quizzes.GetDetailsAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Take(Guid id)
        {
            var model = await quizzes.GetForTakeAsync(id);
            if (model == null) return NotFound();

            if (model.Questions.Count == 0)
            {
                TempData["Error"] = "Този куиз още няма въпроси.";
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(QuizSubmitDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var maxScore = await quizzes.GetQuestionsCountAsync(model.QuizId);
            if (maxScore == 0) return NotFound();

            var chosenAnswerIds = model.Answers.Values.ToList();
            var score = await results.CountCorrectAsync(model.QuizId, chosenAnswerIds);

            var userId = Guid.Parse(userManager.GetUserId(User)!);

            var resultId = await results.SaveResultAsync(model.QuizId, userId, score);

            return RedirectToAction(nameof(Result), new { id = resultId });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Result(Guid id)
        {
            var userId = Guid.Parse(userManager.GetUserId(User)!);
            var isAdmin = User.IsInRole("Admin");

            var model = await results.GetResultAsync(id, userId, isAdmin);
            if (model == null) return NotFound();

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            var userId = Guid.Parse(userManager.GetUserId(User)!);
            var model = await results.MyResultsAsync(userId);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? eraId)
        {
            await LoadErasAsync(eraId);
            return View(new QuizFormDto { EraId = eraId ?? Guid.Empty });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuizFormDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadErasAsync(model.EraId);
                return View(model);
            }

            var id = await quizzes.CreateAsync(model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await quizzes.GetForEditAsync(id);
            if (model == null) return NotFound();

            await LoadErasAsync(model.EraId);
            ViewBag.QuizId = id;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuizFormDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadErasAsync(model.EraId);
                ViewBag.QuizId = id;
                return View(model);
            }

            await quizzes.UpdateAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid? eraId)
        {
            await quizzes.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { eraId });
        }

        // Helper method to load eras for dropdowns
        private async Task LoadErasAsync(Guid? selectedEraId)
        {
            var list = await eras.GetAllAsync();
            ViewBag.Eras = list.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = selectedEraId.HasValue && e.Id == selectedEraId.Value
            }).ToList();
        }
    }
}