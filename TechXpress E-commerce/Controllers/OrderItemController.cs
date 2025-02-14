using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Repositories;

namespace TechXpress_E_commerce.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly IRepository<OrderItem> _orderItemRepository;

        public OrderItemController(IRepository<OrderItem> orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        // Get all order items
        public async Task<IActionResult> Index()
        {
            var orderItems = await _orderItemRepository.GetAllAsync();
            return View(orderItems);
        }

        // Get order item by ID
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                await _orderItemRepository.AddAsync(orderItem);
                await _orderItemRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }

        // Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();
            return View(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderItem orderItem)
        {
            if (id != orderItem.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _orderItemRepository.Update(orderItem);
                await _orderItemRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }

        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            _orderItemRepository.Remove(orderItem);
            await _orderItemRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
