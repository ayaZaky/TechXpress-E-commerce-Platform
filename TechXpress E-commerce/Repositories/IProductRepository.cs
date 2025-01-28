using TechXpress_E_commerce.Models;

namespace TechXpress_E_commerce.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();
        Task<Product?> GetProductWithDetailsAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantity);
        Task<IEnumerable<Product>> GetAllProductsWithImagesAndCategoriesAsync();
        Task<Product?> GetProductWithImagesAsync(int productId);
    }
}
