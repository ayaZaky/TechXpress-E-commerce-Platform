namespace TechXpress_E_commerce.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore; 
    using TechXpress_E_commerce.Models.AppDbContext;

    namespace TechXpress.ViewComponents
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
}
