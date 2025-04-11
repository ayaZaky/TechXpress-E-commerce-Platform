using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.AppContext;
using TechXpress.Data.Entities;

namespace TechXpress.Data.ViewComponents
{
    public class WishListSummaryViewComponent :ViewComponent
    {
        private readonly AppDbContext _context;

        public WishListSummaryViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(new List<WishListItem>());
            }
            var userId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.WishListItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Images)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }
    }
}
