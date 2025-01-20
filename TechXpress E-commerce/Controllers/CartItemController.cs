using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Repositories;

namespace TechXpress_E_commerce.Controllers
{
    public class CartItemController : Controller
    {
        private readonly IRepository<CartItem> _cartItemRepository;

        public CartItemController(IRepository<CartItem> cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        // Get all cart items
        public IActionResult Index()
        {
            var cartItems = _cartItemRepository.GetAll()
                                                  .AsQueryable()
                                               .Include(ci => ci.User)
                                               .Include(ci => ci.Product)
                                               .ToList();
            return View(cartItems);
        }
         
        [HttpGet]
        public IActionResult Details(int id)
        {
            var cartItem = _cartItemRepository.GetById(id);
            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }
         
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public IActionResult Create(CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                cartItem.CreatedAt = DateTime.Now;  
                _cartItemRepository.Add(cartItem);
                _cartItemRepository.SaveChanges(); 
                return RedirectToAction(nameof(Index));
            }
            return View(cartItem);
        } 
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cartItem = _cartItemRepository.GetById(id);
            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }
 
        [HttpPost]
        public IActionResult Edit(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _cartItemRepository.Update(cartItem);
                _cartItemRepository.SaveChanges(); 
                return RedirectToAction(nameof(Index));
            }
            return View(cartItem);
        }
         
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cartItem = _cartItemRepository.GetById(id);
            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        } 
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cartItem = _cartItemRepository.GetById(id);
            if (cartItem == null)
                return NotFound();

            _cartItemRepository.Delete(cartItem);
            _cartItemRepository.SaveChanges();  
            return RedirectToAction(nameof(Index));
        }
    }

}

