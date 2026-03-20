using Microsoft.AspNetCore.Mvc;

namespace Webshop.MVC.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
