using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Repositories;
using Webshop.MVC.ViewModels;

namespace Webshop.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        //GET: Order (GetAll)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAll();

            var viewModelList = orders.Select(o => new OrderListViewModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                NumberOfItems = o.OrderLines?.Count ?? 0,
                TotalAmount = o.OrderLines?.Sum(ol => ol.Quantity * ol.UnitPrice) ?? 0
            }).ToList();

            return View(viewModelList);
        }
        //GET: Order/Details/5 (Details - GetOne)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderDetailViewModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
            };

            return View(viewModel);
        }

        // create actie

        // update actie

        // delete actie
    }
}
