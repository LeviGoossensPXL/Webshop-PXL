using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Repositories;

namespace Webshop.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // index actie

        // list actie

        // create actie

        // update actie

        // delete actie
    }
}
