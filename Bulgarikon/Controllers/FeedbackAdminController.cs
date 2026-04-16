using System.Security.Claims;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.ViewModels.FeedbackViewModels;
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
            var dtos = await feedbackService.GetAllAsync();

            var model = dtos.Select(x => new FeedbackListItemViewModel
            {
                Id = x.Id,
                Subject = x.Subject,
                Category = x.Category,
                CreatedAt = x.CreatedAt,
                UserEmail = x.UserEmail,
                AdminReply = x.AdminReply,
                RepliedAt = x.RepliedAt,
                RepliedByEmail = x.RepliedByEmail,
                IsSeenByAdmin = x.IsSeenByAdmin,
                IsReplySeenByUser = x.IsReplySeenByUser
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dto = await feedbackService.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            await feedbackService.MarkSeenByAdminAsync(id);

            var model = new FeedbackDetailsViewModel
            {
                Id = dto.Id,
                Subject = dto.Subject,
                Category = dto.Category,
                Comment = dto.Comment,
                CreatedAt = dto.CreatedAt,
                UserEmail = dto.UserEmail,
                AdminReply = dto.AdminReply,
                RepliedAt = dto.RepliedAt,
                RepliedByEmail = dto.RepliedByEmail,
                IsSeenByAdmin = dto.IsSeenByAdmin,
                IsReplySeenByUser = dto.IsReplySeenByUser
            };

            ViewBag.ReplyModel = new FeedbackReplyViewModel
            {
                FeedbackId = dto.Id,
                AdminReply = dto.AdminReply ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(FeedbackReplyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var dto = await feedbackService.GetDetailsAsync(model.FeedbackId);
                if (dto == null) return NotFound();

                var detailsVm = new FeedbackDetailsViewModel
                {
                    Id = dto.Id,
                    Subject = dto.Subject,
                    Category = dto.Category,
                    Comment = dto.Comment,
                    CreatedAt = dto.CreatedAt,
                    UserEmail = dto.UserEmail,
                    AdminReply = dto.AdminReply,
                    RepliedAt = dto.RepliedAt,
                    RepliedByEmail = dto.RepliedByEmail,
                    IsSeenByAdmin = dto.IsSeenByAdmin,
                    IsReplySeenByUser = dto.IsReplySeenByUser
                };

                ViewBag.ReplyModel = model;
                return View("Details", detailsVm);
            }

            var adminId = GetUserId();

            await feedbackService.ReplyAsync(
                model.FeedbackId,
                adminId,
                model.AdminReply
            );

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