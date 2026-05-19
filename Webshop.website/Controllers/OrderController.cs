using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Webshop.Application.Repositories;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    [Authorize] // All logged-in users can access
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAppUserRepository _userRepository;
        private readonly IIdentityService _identityService;


        public OrderController(IOrderService orderService, IAppUserRepository userRepository, IIdentityService identityService)
        {
            _orderService = orderService;
            _userRepository = userRepository;
            _identityService = identityService;
        }

        // GET: Order/MyOrders (For Customers)
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = await _identityService.EnsureExternalUserAsync(User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _orderService.GetOrdersByUserId(userId);
            var users = await _userRepository.GetAll();

            var viewModelList = result.Data.Select(o => new OrderListViewModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                UserEmail = users.FirstOrDefault(u => u.Id == o.UserId)?.Email ?? "Unknown",
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                TotalAmount = _orderService.CalculateTotalAmount(o)
            }).ToList();

            return View(viewModelList);
        }

        //GET: Order (GetAll) - ADMIN ONLY
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var result = await _orderService.GetAll();
            var users = await _userRepository.GetAll();

            var viewModelList = result.Data.Select(o => new OrderListViewModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                UserEmail = users.FirstOrDefault(u => u.Id == o.UserId)?.Email ?? "Unknown",
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                TotalAmount = _orderService.CalculateTotalAmount(o)
            }).ToList();

            return View(viewModelList);
        }
        //GET: Order/Details/5 (Details - GetOne)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            // Security check: Only Admin or the owner can see the details
            var userId = await _identityService.EnsureExternalUserAsync(User);
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin && result.Data.UserId != userId)
            {
                return Forbid();
            }

            var orderUser = await _userRepository.GetById(result.Data.UserId);

            var viewModel = new OrderDetailViewModel
            {
                OrderId = result.Data.OrderId,
                UserId = result.Data.UserId,
                UserEmail = orderUser?.Email ?? "Unknown",
                OrderDate = result.Data.OrderDate,
                Status = result.Data.Status.ToString(),
                TotalPrice = _orderService.CalculateTotalAmount(result.Data),
                // Format the delivery address safely
                FullAddress = _orderService.GetFormattedDeliveryAddress(result.Data),
                OrderLines = result.Data.OrderLines?.Select(ol => new OrderLineViewModel
                {
                    ProductId = ol.ProductId,
                    Quantity = ol.Quantity,
                    UnitPrice = ol.UnitPrice,
                    ProductName = ol.Product?.Name ?? "Product #" + ol.ProductId
                }).ToList() ?? new List<OrderLineViewModel>()
            };

            return View(viewModel);
        }

        
        // GET: Order/Edit/5 (edit form to open) - ADMIN ONLY
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _orderService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            var orderUser = await _userRepository.GetById(result.Data.UserId);

            // Map Domain Entity to Update ViewModel with rich details
            var model = new OrderUpdateViewModel
            {
                OrderId = result.Data.OrderId,
                UserId = result.Data.UserId,
                UserEmail = orderUser?.Email ?? "Unknown",
                OrderDate = result.Data.OrderDate,
                Status = (int)result.Data.Status,
                FullAddress = _orderService.GetFormattedDeliveryAddress(result.Data),
                TotalAmount = _orderService.CalculateTotalAmount(result.Data),
                OrderLines = result.Data.OrderLines?.Select(ol => new OrderLineViewModel
                {
                    ProductId = ol.ProductId,
                    Quantity = ol.Quantity,
                    UnitPrice = ol.UnitPrice,
                    ProductName = ol.Product?.Name ?? "Product #" + ol.ProductId
                }).ToList() ?? new List<OrderLineViewModel>()
            };

            return View(model);
        }

        // POST: Order/Edit/5 (Save edit) - ADMIN ONLY
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(OrderUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // extract the actual integer value, default to 0 (Pending) if it is somehow null
                int statusValue = model.Status ?? 0;

                var result = await _orderService.UpdateOrderStatus(model.OrderId, statusValue);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault());
                    return View(model);
                }

                return RedirectToAction("Index");
            }

            // If error, show the form again
            return View(model);
        }

        // GET: Order/Delete/5 (delete confirm page) - ADMIN ONLY
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            // We show the details to ask "Are you sure?"
            var viewModel = new OrderDetailViewModel
            {
                OrderId = result.Data.OrderId,
                UserId = result.Data.UserId,
                OrderDate = result.Data.OrderDate,
                Status = result.Data.Status.ToString(),
                TotalPrice = _orderService.CalculateTotalAmount(result.Data),
                FullAddress = _orderService.GetFormattedDeliveryAddress(result.Data)
            };

            return View(viewModel);
        }

        // POST: Order/Delete/5  - ADMIN ONLY
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.Delete(id);
            
            return RedirectToAction(nameof(Index));
        }

    }
}
