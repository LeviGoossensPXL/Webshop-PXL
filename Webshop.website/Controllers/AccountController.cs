using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webshop.website.ViewModels;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;

namespace Webshop.website.Controllers
{
    public class AccountController : Controller
    {

        private readonly IIdentityService _identityService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IIdentityService identityService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _identityService = identityService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Show the login page
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Save the return URL so the form can use it
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Process the login request
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {

                var result = await _identityService.SignInAsync(model.Email, model.Password);


                if (result.SignInResult != null && result.SignInResult.Succeeded)
                {
                    // If we have a safe return URL, send the user back there
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Otherwise, send them to the shop
                    return RedirectToAction("Index", "Shop");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            // Keep the return URL if login fails
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // Show the register page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Process the register request
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _identityService.RegisterAsync(model.Email, model.Password);

                if (result.IdentityResult != null && result.IdentityResult.Succeeded)
                {
                    // If registration was successful, automatically log the user in
                    await _identityService.SignInAsync(model.Email, model.Password);

                    // Go to the shop page
                    return RedirectToAction("Index", "Shop");
                }

                // If the save operation failed, display the errors on screen
                if (result.IdentityResult != null && result.IdentityResult.Errors != null)
                {
                    foreach (var error in result.IdentityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // We are logging out via IdentityService
            await _identityService.SignOutAsync();
            // Clear the session memory (now this will empty the shopping cart)
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Shop");
        }

        public IActionResult GoogleLogin()
        {
            string? redirectUrl = Url.Action("GoogleResponse");
            string scheme = "oidc";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(
                scheme, redirectUrl);
            return new ChallengeResult(scheme, properties);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo? externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);
            if (user == null)
            {
                user = await CreateIdentityUserFromClaims(externalLoginInfo);
            }
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");
        }

        private async Task<AppUser?> CreateIdentityUserFromClaims(ExternalLoginInfo externalLoginInfo)
        {
            var claim = externalLoginInfo.Principal.FindFirst(ClaimTypes.Email);
            if (claim == null) return null;
            var email = claim.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser {UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return null;
                }
            }
            var loginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
            if (!loginResult.Succeeded) return null;
            return user;
        }
    }
}