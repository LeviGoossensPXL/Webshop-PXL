using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Controllers
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
            var order = await _orderRepository.GetById(id);
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
                var order = await _orderRepository.GetById(model.OrderId);
                if (order != null)
                {
                    order.Status = (OrderStatus)model.Status;
                    await _orderRepository.Update(order);
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
            var order = await _orderRepository.GetById(id);
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
                TotalPrice = order.OrderLines?.Sum(ol => ol.Quantity * ol.UnitPrice) ?? 0,
                FullAddress = order.DeliveryAddress != null
                    ? $"{order.DeliveryAddress.Street} {order.DeliveryAddress.HouseNumber}, {order.DeliveryAddress.ZipCode} {order.DeliveryAddress.City}, {order.DeliveryAddress.Country}"
                    : "No address provided"
            };

            return View(viewModel);
        }

        // POST: Order/Delete/5 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderRepository.GetById(id);

            if (order != null)
            {
                // Database cleanup
                await _orderRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
