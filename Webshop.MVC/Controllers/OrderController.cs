using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;
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

        // GET: Order/Create (create form to open )
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create (for the form save )
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map ViewModel to Domain Entity
                var order = new Order
                {
                    OrderDate = System.DateTime.UtcNow,
                    Status = OrderStatus.Pending // Use the enum instead of an integer
                };

                await _orderRepository.Add(order);
                return RedirectToAction("Index"); // Return to list after saving
            }

            // If there is a validation error, show the form again
            return View(model);
        }
        // update actie

        // delete actie
    }
}
