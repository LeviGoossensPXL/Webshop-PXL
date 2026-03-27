using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

       
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Process the login request
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Try to sign in the user
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                   
                    return RedirectToAction("Index", "Shop");
                }

             
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
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