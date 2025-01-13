using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace TechXpress_E_commerce.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly AppDbContext _context;

        public OrderItemController(AppDbContext context)
        {
            _context = context;
        }

        // Get all order items
        public IActionResult Index()
        {
            var orderItems = _context.OrderItems
                                     .Include(oi => oi.Order)
                                     .Include(oi => oi.Product)
                                     .ToList();
            return View(orderItems);
        }

        // Get order item by ID
        [HttpGet]
        public IActionResult Details(int id)
        {
            var orderItem = _context.OrderItems
                                     .Include(oi => oi.Order)
                                     .Include(oi => oi.Product)
                                     .FirstOrDefault(oi => oi.Id == id);

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
                _context.OrderItems.Add(orderItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            } 
            return View(orderItem);
        }

        // Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var orderItem = _context.OrderItems.Find(id);
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
                _context.Update(orderItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            } 
            return View(orderItem);
        }

        // Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var orderItem = _context.OrderItems
                                     .Include(oi => oi.Order)
                                     .Include(oi => oi.Product)
                                     .FirstOrDefault(oi => oi.Id == id);
            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var orderItem = _context.OrderItems.Find(id);
            if (orderItem == null)
                return NotFound();

            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
