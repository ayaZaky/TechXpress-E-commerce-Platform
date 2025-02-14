using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Repositories;
using System.Security.Claims;

namespace TechXpress_E_commerce.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IRepository<Product> _productRepository;

        public CartItemController(ICartItemRepository cartItemRepository, IRepository<Product> productRepository)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartItemRepository.GetCartItemsAsync(userId);
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // إعادة التوجيه لصفحة تسجيل الدخول
            }
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Add or update the cart item
            var cartItem = await _cartItemRepository.UpdateOrAddToCartAsync(productId, userId, quantity);

            // Get updated cart count and save it in TempData
            var cartCount = await _cartItemRepository.GetCartItemCountAsync(userId);
            TempData["CartCount"] = cartCount;

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartItemRepository.RemoveFromCartAsync(id, userId);
            return RedirectToAction("Index", "CartItem");
           
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = await _cartItemRepository.GetCartItemAsync(userId, id);

            if (cartItem != null && quantity > 0)
            {
                await _cartItemRepository.UpdateQuantityAsync(id, userId, quantity);
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
