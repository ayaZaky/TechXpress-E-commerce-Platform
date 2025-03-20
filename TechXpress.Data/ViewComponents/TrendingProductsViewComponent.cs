using TechXpress.Data.AppContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TechXpress.Data.ViewComponents
    {
        public class TrendingProductsViewComponent : ViewComponent
        {
            private readonly  AppDbContext _context;

            public TrendingProductsViewComponent(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IViewComponentResult> InvokeAsync(int count = 8)
            {
                var trendingProducts = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .OrderByDescending(p => p.OrderItems.Sum(oi => oi.Quantity))
                    .Take(count)
                    .ToListAsync();

                return View(trendingProducts);
            }
        }
    }

