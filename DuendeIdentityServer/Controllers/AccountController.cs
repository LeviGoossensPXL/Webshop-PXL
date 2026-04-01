using DuendeIdentityServer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DuendeIdentityServer.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                // If login is successful, we will redirect the user back to the webshop using model.ReturnUrl

                ModelState.AddModelError("", "Database validation is not yet connected.");
            }

            return View(model);
        }
    }
}
