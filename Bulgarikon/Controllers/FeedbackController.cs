using Bulgarikon.Core.DTOs.FeedbackDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.ViewModels.FeedbackViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
            var model = new FeedbackCreateViewModel
            {
                Category = FeedbackCategory.Idea
            };

            ViewBag.Categories = BuildCategorySelectList(model.Category);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = BuildCategorySelectList(model.Category);
                return View(model);
            }

            var userId = GetUserId();

            // map VM -> DTO (service layer)
            var dto = new FeedbackCreateDto
            {
                Subject = model.Subject,
                Category = model.Category,
                Comment = model.Comment
            };

            await feedbackService.CreateAsync(userId, dto);

            TempData["Ok"] = "Feedback-ът е изпратен успешно.";
            return RedirectToAction(nameof(My));
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var userId = GetUserId();

            var dtos = await feedbackService.GetMineAsync(userId);

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

            await feedbackService.MarkUserRepliesSeenAsync(userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var userId = GetUserId();

            var dto = await feedbackService.GetDetailsAsync(id);
            if (dto == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var isOwner = await feedbackService.IsOwnerAsync(id, userId);
                if (!isOwner) return Forbid();
            }

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

            return View(model);
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdStr, out var userId))
                throw new InvalidOperationException("Invalid user id.");

            return userId;
        }

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