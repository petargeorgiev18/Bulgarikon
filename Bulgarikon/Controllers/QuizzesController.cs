using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models;
using Bulgarikon.ViewModels.QuizViewModels;
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

            var vm = model.Select(x => new QuizViewViewModel
            {
                Id = x.Id,
                Title = x.Title,
                EraId = x.EraId,
                EraName = x.EraName,
                QuestionsCount = x.QuestionsCount
            }).ToList();

            ViewBag.EraId = eraId;
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dto = await quizzes.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            var vm = new QuizDetailsViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                EraId = dto.EraId,
                EraName = dto.EraName,
                QuestionsCount = dto.QuestionsCount
            };

            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Take(Guid id)
        {
            var dto = await quizzes.GetForTakeAsync(id);
            if (dto == null) return NotFound();

            if (dto.Questions == null || dto.Questions.Count == 0)
            {
                TempData["Error"] = "Този куиз още няма въпроси.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var vm = new QuizTakeViewModel
            {
                QuizId = dto.QuizId,
                Title = dto.Title,
                EraName = dto.EraName,
                Questions = dto.Questions.Select(q => new QuizTakeQuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new QuizTakeAnswerViewModel
                    {
                        AnswerId = a.AnswerId,
                        Text = a.Text
                    }).ToList()
                }).ToList()
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(QuizSubmitViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var take = await quizzes.GetForTakeAsync(model.QuizId);
            if (take == null) return NotFound();

            var maxScore = take.Questions.Count;

            if (model.Answers == null || model.Answers.Count != maxScore)
            {
                TempData["Error"] = "Моля, отговори на всички въпроси.";
                return RedirectToAction(nameof(Take), new { id = model.QuizId });
            }

            var allowed = take.Questions
                .SelectMany(q => q.Answers)
                .Select(a => a.AnswerId)
                .ToHashSet();

            var chosen = model.Answers.Values
                .Where(a => allowed.Contains(a))
                .ToList();

            if (chosen.Count != maxScore)
            {
                TempData["Error"] = "Невалидни отговори.";
                return RedirectToAction(nameof(Take), new { id = model.QuizId });
            }

            var score = await results.CountCorrectAsync(model.QuizId, chosen);

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

            var dto = await results.GetResultAsync(id, userId, isAdmin);
            if (dto == null) return NotFound();

            var vm = new QuizResultViewViewModel
            {
                ResultId = dto.ResultId,
                Id = dto.Id,
                QuizId = dto.QuizId,
                QuizTitle = dto.QuizTitle,
                EraName = dto.EraName,
                Score = dto.Score,
                MaxScore = dto.MaxScore,
                DateTaken = dto.DateTaken
            };

            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            var userId = Guid.Parse(userManager.GetUserId(User)!);

            var dtos = await results.MyResultsAsync(userId);

            var vm = dtos.Select(x => new QuizResultListItemViewModel
            {
                Id = x.Id,
                QuizTitle = x.QuizTitle,
                Score = x.Score,
                MaxScore = x.MaxScore,
                DateTaken = x.DateTaken
            }).ToList();

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? eraId)
        {
            await LoadErasAsync(eraId);

            return View(new QuizFormViewModel
            {
                EraId = eraId ?? Guid.Empty
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuizFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadErasAsync(model.EraId);
                return View(model);
            }

            var id = await quizzes.CreateAsync(new Core.DTOs.QuizDTOs.QuizFormDto
            {
                Title = model.Title,
                EraId = model.EraId
            });

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dto = await quizzes.GetForEditAsync(id);
            if (dto == null) return NotFound();

            await LoadErasAsync(dto.EraId);

            ViewBag.QuizId = id;

            return View(new QuizFormViewModel
            {
                Title = dto.Title,
                EraId = dto.EraId
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuizFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadErasAsync(model.EraId);
                ViewBag.QuizId = id;
                return View(model);
            }

            await quizzes.UpdateAsync(id, new Core.DTOs.QuizDTOs.QuizFormDto
            {
                Title = model.Title,
                EraId = model.EraId
            });

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, Guid? eraId)
        {
            await quizzes.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { eraId });
        }

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