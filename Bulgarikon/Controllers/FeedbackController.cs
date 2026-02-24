using System.Security.Claims;
using Bulgarikon.Core.DTOs.FeedbackDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulgarikon.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new FeedbackCreateDto { Category = FeedbackCategory.Idea };
            ViewBag.Categories = BuildCategorySelectList(model.Category);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = BuildCategorySelectList(model.Category);
                return View(model);
            }

            var userId = GetUserId();
            await feedbackService.CreateAsync(userId, model);

            TempData["Ok"] = "Feedback-ът е изпратен успешно.";
            return RedirectToAction(nameof(My));
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var userId = GetUserId();

            var list = await feedbackService.GetMineAsync(userId);

            await feedbackService.MarkUserRepliesSeenAsync(userId);

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var userId = GetUserId();

            var model = await feedbackService.GetDetailsAsync(id);
            if (model == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var isOwner = await feedbackService.IsOwnerAsync(id, userId);
                if (!isOwner) return Forbid();
            }

            return View(model);
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                throw new InvalidOperationException("Invalid user id.");
            return userId;
        }

        // Helper method to build the select list for feedback categories
        private static List<SelectListItem> BuildCategorySelectList(FeedbackCategory selected)
        {
            return new List<SelectListItem>
            {
                new() { Value = FeedbackCategory.Idea.ToString(), Text = "Идея", Selected = selected == FeedbackCategory.Idea },
                new() { Value = FeedbackCategory.Bug.ToString(), Text = "Грешка", Selected = selected == FeedbackCategory.Bug },
                new() { Value = FeedbackCategory.Question.ToString(), Text = "Въпрос", Selected = selected == FeedbackCategory.Question },
                new() { Value = FeedbackCategory.Other.ToString(), Text = "Друго", Selected = selected == FeedbackCategory.Other },
            };
        }
    }
}