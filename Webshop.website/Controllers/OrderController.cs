using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Webshop.Application.Repositories;
using Webshop.Application.Services.Contracts;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //GET: Order (GetAll)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAll();

            var viewModelList = orders.Select(o => new OrderListViewModel
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
            var order = await _orderService.GetById(id);
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
                TotalPrice = _orderService.CalculateTotalAmount(order),
                // Format the delivery address safely
                FullAddress = _orderService.GetFormattedDeliveryAddress(order)
            };

            return View(viewModel);
        }

        
        // GET: Order/Edit/5 (edit form to open)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            // Map Domain Entity to Update ViewModel
            var model = new OrderUpdateViewModel
            {
                OrderId = order.OrderId,
                Status = (int)order.Status
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

                await _orderService.UpdateOrderStatus(model.OrderId, statusValue);

                return RedirectToAction("Index");
            }

            // If error, show the form again
            return View(model);
        }

        // GET: Order/Delete/5 (delete confirm page)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            // We show the details to ask "Are you sure?"
            var viewModel = new OrderDetailViewModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalPrice = _orderService.CalculateTotalAmount(order),
                FullAddress = _orderService.GetFormattedDeliveryAddress(order)
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
