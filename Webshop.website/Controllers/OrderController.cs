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
                TotalAmount = o.OrderLines?.Sum(ol => ol.Quantity * ol.UnitPrice) ?? 0
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
                TotalPrice = order.OrderLines?.Sum(ol => ol.Quantity * ol.UnitPrice) ?? 0,
                // Format the delivery address safely
                FullAddress = order.DeliveryAddress != null
                    ? $"{order.DeliveryAddress.Street} {order.DeliveryAddress.HouseNumber}, {order.DeliveryAddress.ZipCode} {order.DeliveryAddress.City}, {order.DeliveryAddress.Country}"
                    : "No address provided"
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
                var order = await _orderService.GetById(model.OrderId);
                if (order != null)
                {
                    order.Status = (OrderStatus)model.Status;
                    await _orderService.Update(order);
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
                TotalPrice = order.OrderLines?.Sum(ol => ol.Quantity * ol.UnitPrice) ?? 0, //TODO move logic like this to service
                FullAddress = order.DeliveryAddress != null
                    ? $"{order.DeliveryAddress.Street} {order.DeliveryAddress.HouseNumber}, {order.DeliveryAddress.ZipCode} {order.DeliveryAddress.City}, {order.DeliveryAddress.Country}"
                    : "No address provided"//TODO move logic like this to service
            };

            return View(viewModel);
        }

        // POST: Order/Delete/5 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderService.GetById(id);//TODO begin

            if (order != null)
            {
                // Database cleanup
                await _orderService.Delete(id);
            }//TODO end   [move logic like this to service]

            return RedirectToAction(nameof(Index));
        }

    }
}
