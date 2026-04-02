using DuendeIdentityServer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Services.Contracts;

namespace DuendeIdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
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

                var result = await _identityService.SignInAsync(model.Username, model.Password);

                if (result.Succeeded && result.SignInResult != null && result.SignInResult.Succeeded)
                {
                    // verify if the return url belongs to our application to prevent redirect attacks
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        // fallback to the application root if the return url is missing
                        return Redirect("~/");
                    }
                    else
                    {
                        throw new Exception("Invalid return URL.");
                    }
                }

                // bind any authentication errors to the model state to show them on the ui
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            return View(model);
        }
    }
}
