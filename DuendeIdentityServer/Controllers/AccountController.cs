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

                var result = await _identityService.SignInAsync(model.Email, model.Password);

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

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var model = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // call your existing service implementation!
                var result = await _identityService.RegisterAsync(model.Email, model.Password);

                // check the custom IdentityRegisterResult wrapper you built
                if (result.IdentityResult != null && result.IdentityResult.Succeeded)
                {
                    // sign the user in immediately after successful registration
                    await _identityService.SignInAsync(model.Email, model.Password);

                    // redirect back to the webshop using the safe return url
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        throw new Exception("Invalid return URL.");
                    }
                }

                // bind any validation errors (like weak passwords) to the model state
                if (result.IdentityResult != null)
                {
                    foreach (var error in result.IdentityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else if (result.Errors != null && result.Errors.Any())
                {
                    // fallback in case errors were added to your custom wrapper directly
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            return View(model);
        }
    }
}
