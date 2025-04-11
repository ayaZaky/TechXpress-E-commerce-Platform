using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Entities;   

using TechXpress.Data.AppContext;

namespace TechXpress.Data.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly  AppDbContext _context;

        public CartSummaryViewComponent( AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(new List<CartItem>());
            }
            var userId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier); 
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Images)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }
    }
}

