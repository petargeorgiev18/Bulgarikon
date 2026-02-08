using Bulgarikon.Core.DTOs.QuestionDTOs;
using Bulgarikon.Core.DTOs.QuizDTOs;
using Bulgarikon.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulgarikon.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService questions;
        private readonly IQuizService quizzes;

        public QuestionsController(IQuestionService questions, IQuizService quizzes)
        {
            this.questions = questions;
            this.quizzes = quizzes;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid quizId)
        {
            var quiz = await quizzes.GetDetailsAsync(quizId);
            if (quiz == null) return NotFound();

            ViewBag.QuizId = quizId;
            ViewBag.QuizTitle = quiz.Title;

            var model = await questions.GetByQuizAsync(quizId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create(Guid quizId)
        {
            return View(new QuestionFormDto { QuizId = quizId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionFormDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await questions.CreateAsync(model);
                return RedirectToAction(nameof(Index), new { quizId = model.QuizId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await questions.GetForEditAsync(id);
            if (model == null) return NotFound();

            ViewBag.QuestionId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuestionFormDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.QuestionId = id;
                return View(model);
            }

            try
            {
                await questions.UpdateAsync(id, model);
                return RedirectToAction(nameof(Index), new { quizId = model.QuizId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.QuestionId = id;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid quizId)
        {
            await questions.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { quizId });
        }
    }
}