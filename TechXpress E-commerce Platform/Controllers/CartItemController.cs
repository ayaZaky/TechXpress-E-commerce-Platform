using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TechXpress.Services.Interfaces;

namespace TechXpress_E_commerce_Platform.Controllers
{
    public class CartItemController : Controller
    {

        private readonly ICartService _cartItemService;
        private readonly IProductService _productService;

        public CartItemController(ICartService cartItemService, IProductService productService)
        {
            _cartItemService = cartItemService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartItemService.GetCartItemsAsync(userId);
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

            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Add or update the cart item
            await _cartItemService.UpdateOrAddToCartAsync(productId, userId, quantity);

            // Get updated cart count and save it in TempData
            var cartCount = await _cartItemService.GetCartItemCountAsync(userId);
            TempData["CartCount"] = cartCount;

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartItemService.RemoveFromCartAsync(id, userId);
            return RedirectToAction("Index", "CartItem");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = await _cartItemService.GetCartItemAsync(userId, id);

            if (cartItem != null && quantity > 0)
            {
                await _cartItemService.UpdateQuantityAsync(id, userId, quantity);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
