using Duende.IdentityServer.Services;
using DuendeIdentityServer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DuendeIdentityServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(
            IIdentityServerInteractionService interaction,
            ILogger<HomeController> logger,
            IWebHostEnvironment environment)
        {
            _interaction = interaction;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error(string? errorId)
        {
            var model = new ErrorViewModel();
            var error = string.IsNullOrWhiteSpace(errorId)
                ? null
                : await _interaction.GetErrorContextAsync(errorId);

            if (error != null)
            {
                _logger.LogWarning(
                    "IdentityServer login failed: {Error}. {ErrorDescription}. Request ID: {RequestId}",
                    error.Error, error.ErrorDescription, error.RequestId);

                model.Error = error.Error;
                model.RequestId = error.RequestId;
                if (_environment.IsDevelopment())
                {
                    model.ErrorDescription = error.ErrorDescription;
                }
            }

            return View(model);
        }
    }
}
