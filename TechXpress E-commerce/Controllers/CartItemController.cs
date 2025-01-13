using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace TechXpress_E_commerce.Controllers
{
    public class CartItemController : Controller
    {
        private readonly AppDbContext _context;

        public CartItemController(AppDbContext context)
        {
            _context = context;
        }

        // Get all cart items
        public IActionResult Index()
        {
            var cartItems = _context.CartItems
                                    .Include(ci => ci.User)
                                    .Include(ci => ci.Product)
                                    .ToList();
            return View(cartItems);
        }

        // Get cart item by ID
        [HttpGet]
        public IActionResult Details(int id)
        {
            var cartItem = _context.CartItems
                                   .Include(ci => ci.User)
                                   .Include(ci => ci.Product)
                                   .FirstOrDefault(ci => ci.Id == id);

            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }

        // Create (GET)
        [HttpGet]
        public IActionResult Create()
        { 
            return View();
        }

        // Create (POST)
        [HttpPost]
        public IActionResult Create(CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                cartItem.CreatedAt = DateTime.Now; // Set CreatedAt to the current time
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            } 
            return View(cartItem);
        }

        // Edit (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cartItem = _context.CartItems.Find(id);
            if (cartItem == null)
                return NotFound();
             
            return View(cartItem);
        }

        // Edit (POST)
        [HttpPost]
        public IActionResult Edit(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(cartItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            } 
            return View(cartItem);
        }

        // Delete (GET)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cartItem = _context.CartItems
                                   .Include(ci => ci.User)
                                   .Include(ci => ci.Product)
                                   .FirstOrDefault(ci => ci.Id == id);
            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }

        // Delete (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cartItem = _context.CartItems.Find(id);
            if (cartItem == null)
                return NotFound();

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

}

