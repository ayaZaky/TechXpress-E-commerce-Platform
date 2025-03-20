using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.AppContext;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Implementation;
using TechXpress.Data.Repositories.Interfaces;
 
namespace TechXpress_E_commerce.Repositories
{                                                                 
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }
     

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
            // Filter methods
            public async Task<IEnumerable<Product>> FilterProductsAsync(
              string? searchTerm = null,
              decimal? minPrice = null,
              decimal? maxPrice = null,
              string? brand = null,
              double? minRating = null,
              bool? isAvailable = null,
              string? sortBy = null,
              bool ascending = true,
              int? categoryId = null)
            {
                var query = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(searchTerm) ||
                        p.Description!.ToLower().Contains(searchTerm) ||
                        p.Brand!.ToLower().Contains(searchTerm));
                }

                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= maxPrice.Value);
                }

                if (!string.IsNullOrWhiteSpace(brand))
                {
                    query = query.Where(p => p.Brand!.ToLower() == brand.ToLower());
                }

                if (minRating.HasValue)
                {
                    query = query.Where(p => p.Rating >= minRating.Value);
                }

                if (isAvailable.HasValue)
                {
                    query = query.Where(p => p.IsAvailable == isAvailable.Value);
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                // Apply sorting
                query = ApplySorting(query, sortBy, ascending);

                return await query.ToListAsync();
            }

            public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsAvailable)
                    .OrderBy(p => p.Price)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p => p.Brand!.ToLower() == brand.ToLower() && p.IsAvailable)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Product>> GetTopRatedProductsAsync(double minRating = 4.0)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p => p.Rating >= minRating && p.IsAvailable)
                    .OrderByDescending(p => p.Rating)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
            {
                searchTerm = searchTerm.ToLower();
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p =>
                        p.IsAvailable &&
                        (p.Name.ToLower().Contains(searchTerm) ||
                         p.Description!.ToLower().Contains(searchTerm) ||
                         p.Brand!.ToLower().Contains(searchTerm)))
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int days = 30)
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-days);
                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p => p.CreatedAt >= cutoffDate && p.IsAvailable)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
            }

            private IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sortBy, bool ascending)
            {
                Expression<Func<Product, object>> keySelector = sortBy?.ToLower() switch
                {
                    "price" => p => p.Price,
                    "name" => p => p.Name,
                    "rating" => p => p.Rating,
                    "created" => p => p.CreatedAt,
                    _ => p => p.Name // Default sorting by name
                };

                return ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
            }


        }
    }

