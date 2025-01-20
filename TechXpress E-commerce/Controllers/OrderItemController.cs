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
        public IActionResult Index()
        {
            var orderItems = _orderItemRepository.GetAll().AsQueryable()
                                                 .Include(oi => oi.Order)
                                                 .Include(oi => oi.Product)
                                                 .ToList();
            return View(orderItems);
        }

        // Get order item by ID
        [HttpGet]
        public IActionResult Details(int id)
        {
            var orderItem = _orderItemRepository.GetById(id);
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
        public IActionResult Create(OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _orderItemRepository.Add(orderItem);
                _orderItemRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }

        // Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var orderItem = _orderItemRepository.GetById(id);
            if (orderItem == null)
                return NotFound();
            return View(orderItem);
        }

        [HttpPost]
        public IActionResult Edit(int id, OrderItem orderItem)
        {
            if (id != orderItem.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _orderItemRepository.Update(orderItem);
                _orderItemRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }

        // Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var orderItem = _orderItemRepository.GetById(id);
            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var orderItem = _orderItemRepository.GetById(id);
            if (orderItem == null)
                return NotFound();

            _orderItemRepository.Delete(orderItem);
            _orderItemRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
