using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TechXpress.Services.Interfaces;

namespace TechXpress_E_commerce_Platform.Controllers
{
    public class WishListController : Controller
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishListItems = await _wishListService.GetWishListItemsAsync(userId);
            return View(wishListItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishList(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishListItem = await _wishListService.AddToWishListAsync(productId, userId);

            if (wishListItem == null)
            {
                TempData["Message"] = "Product is already in your wish list!";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishList(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _wishListService.RemoveFromWishListAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}

