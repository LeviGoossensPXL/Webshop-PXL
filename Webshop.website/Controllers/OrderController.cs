using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Webshop.Application.Repositories;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can access this controller
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        //GET: Order (GetAll)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _orderService.GetAll();

            var viewModelList = result.Data.Select(o => new OrderListViewModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
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
            // rely on the explicit result status rather than null checks
            if (!result.Succeeded)
            {
                return NotFound();
            }

            var order = result.Data;
            var orderItemsList = new List<OrderItemViewModel>();

            if (order.OrderLines != null && order.OrderLines.Any())
            {
                foreach (var line in order.OrderLines)
                {
                    
                    string productName = $"Product #{line.ProductId}";

                    
                    var productResult = await _productService.GetById(line.ProductId);
                    if (productResult.Succeeded && productResult.Data != null)
                    {
                        productName = productResult.Data.Name;
                    }

                    orderItemsList.Add(new OrderItemViewModel
                    {
                        ProductId = line.ProductId,
                        ProductName = productName,
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice
                    });
                }
            }

            var viewModel = new OrderDetailViewModel
            {
                OrderId = result.Data.OrderId,
                UserId = result.Data.UserId,
                OrderDate = result.Data.OrderDate,
                Status = result.Data.Status.ToString(),
                TotalPrice = _orderService.CalculateTotalAmount(result.Data),
                // Format the delivery address safely
                FullAddress = _orderService.GetFormattedDeliveryAddress(result.Data),
                Items = orderItemsList
            };

            return View(viewModel);
        }

        
        // GET: Order/Edit/5 (edit form to open)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _orderService.GetById(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            // Map Domain Entity to Update ViewModel
            var model = new OrderUpdateViewModel
            {
                OrderId = result.Data.OrderId,
                Status = (int)result.Data.Status
            };

            return View(model);
        }

        // POST: Order/Edit/5 (Save edit)
        [HttpPost]
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

        // GET: Order/Delete/5 (delete confirm page)
        [HttpGet]
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

        // POST: Order/Delete/5 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.Delete(id);
            
            return RedirectToAction(nameof(Index));
        }

    }
}
