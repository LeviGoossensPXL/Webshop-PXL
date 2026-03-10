using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Repositories;

namespace Webshop.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // index actie

        // list actie

        // create actie

        // update actie

        // delete actie
    }
}
