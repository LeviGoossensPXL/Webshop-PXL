using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webshop.website.ViewModels;
using Webshop.Domain.Entities;

namespace Webshop.website.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
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
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
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
                // Create a new user
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Sign in the new user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Go to the shop page
                    return RedirectToAction("Index", "Shop");
                }

                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Shop");
        }
    }
}