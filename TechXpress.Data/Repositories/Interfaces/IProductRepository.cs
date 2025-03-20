using TechXpress.Data.Entities;

namespace TechXpress.Data.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
       
        // Basic CRUD operations
        Task<IEnumerable<Product>> GetAllProductsWithImagesAndCategoriesAsync();
        Task<Product?> GetProductWithImagesAsync(int productId);
        Task<Product?> GetProductWithDetailsAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantity);

        // Category-based queries
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();

        // Filter methods
      
        Task<IEnumerable<Product>> FilterProductsAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brand = null,
            double? minRating = null,
            bool? isAvailable = null,
            string? sortBy = null,
            bool ascending = true,
            int? categoryId = null);

        // Specialized filter methods
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand);
        Task<IEnumerable<Product>> GetTopRatedProductsAsync(double minRating = 4.0);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Product>> GetNewArrivalsAsync(int days = 30);
    }
}
