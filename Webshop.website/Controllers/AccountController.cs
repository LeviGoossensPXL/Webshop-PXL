using Microsoft.AspNetCore.Mvc;
using Webshop.website.ViewModels;
using Webshop.Application.Services.Contracts;

namespace Webshop.website.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
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
                    // If the user is an admin, redirect to Product; otherwise, redirect to Shop
                    if (await _identityService.IsInRoleAsync(model.Email, "Admin"))
                    {
                        return RedirectToAction("Index", "Product");
                    }

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
            var properties = _identityService.GoogleLogin(Url.Action("GoogleResponse")!);
            return new ChallengeResult("google", properties);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await _identityService.GoogleResponse();

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return RedirectToAction("Login");
            }

            return RedirectToAction("PostLogin");
        }

        [HttpGet]
        public IActionResult LoginWithDuende(string returnUrl = "/")
        {
          
            // build an absolute uri for the authentication properties to prevent request uri errors
            var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                RedirectUri = Url.Action("PostLogin", new { returnUrl })
            };

            // challenge the oidc scheme to trigger the redirect to duende identityserver
            return Challenge(properties, "oidc");
        }

        [HttpGet]
        public async Task<IActionResult> PostLogin(string returnUrl = "/")
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                // Ensure the external user exists in our local database
                await _identityService.EnsureExternalUserAsync(User);

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Product");
                }

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }
            return RedirectToAction("Index", "Shop");
        }
    }
}