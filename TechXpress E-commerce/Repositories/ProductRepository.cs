using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Models.AppDbContext;

namespace TechXpress_E_commerce.Repositories
{ 
    public class ProductRepository : IProductRepository
        {
            protected readonly AppDbContext _context; 

            public ProductRepository(AppDbContext context)
            {
                _context = context;
                 
            } 
            public async Task<IEnumerable<Product>> GetAllProductsWithImagesAndCategoriesAsync()
            {
                return await  _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images) 
                    .ToListAsync();
            }

            public async Task<Product?> GetProductWithImagesAsync(int productId)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == productId);
            }

            public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p => p.CategoryId == categoryId && p.IsAvailable)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p => p.IsFeatured && p.IsAvailable)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }

            public async Task<Product?> GetProductWithDetailsAsync(int productId)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == productId && p.IsAvailable);
            }

            public async Task<bool> UpdateStockAsync(int productId, int quantity)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null || product.StockQuantity < quantity)
                {
                    return false;
                }

                product.StockQuantity -= quantity;
                return true;
            }
         
    }
}

