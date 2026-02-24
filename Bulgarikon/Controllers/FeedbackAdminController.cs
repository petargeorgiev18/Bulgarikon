using System.Security.Claims;
using Bulgarikon.Core.DTOs.FeedbackDTOs;
using Bulgarikon.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulgarikon.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeedbackAdminController : Controller
    {
        private readonly IFeedbackService feedbackService;

        public FeedbackAdminController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await feedbackService.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await feedbackService.GetDetailsAsync(id);
            if (model == null) return NotFound();

            await feedbackService.MarkSeenByAdminAsync(id);

            ViewBag.ReplyModel = new FeedbackReplyDto
            {
                FeedbackId = id,
                AdminReply = model.AdminReply ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(FeedbackReplyDto model)
        {
            if (!ModelState.IsValid)
            {
                var details = await feedbackService.GetDetailsAsync(model.FeedbackId);
                if (details == null) return NotFound();

                ViewBag.ReplyModel = model;
                return View("Details", details);
            }

            var adminId = GetUserId();

            await feedbackService.ReplyAsync(model.FeedbackId, adminId, model.AdminReply);

            TempData["Ok"] = "Отговорът е изпратен.";
            return RedirectToAction(nameof(Details), new { id = model.FeedbackId });
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                throw new InvalidOperationException("Invalid user id.");
            return userId;
        }
    }
}