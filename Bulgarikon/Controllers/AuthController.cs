using Bulgarikon.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bulgarikon.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<BulgarikonUser> signInManager;
        private readonly UserManager<BulgarikonUser> userManager;

        public AuthController(
            SignInManager<BulgarikonUser> signInManager,
            UserManager<BulgarikonUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });

            var signInResult = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
                return LocalRedirect(returnUrl);

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new BulgarikonUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });

                await userManager.AddToRoleAsync(user, "User");
            }

            var addLoginResult = await userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                TempData["Error"] = string.Join(" | ", addLoginResult.Errors.Select(e => e.Description));
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
            }


            await signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }
    }
}
