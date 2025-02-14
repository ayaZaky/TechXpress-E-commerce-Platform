using TechXpress_E_commerce.Models;

namespace TechXpress_E_commerce.Repositories
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
        Task<CartItem> AddToCartAsync(int productId, string userId, int quantity);
        Task<CartItem> UpdateOrAddToCartAsync(int productId, string userId, int quantity);
        Task RemoveFromCartAsync(int cartItemId, string userId);
        Task UpdateQuantityAsync(int cartItemId, string userId, int quantity);
        Task<int> GetCartItemCountAsync(string userId);
        Task<CartItem> GetCartItemAsync(string userId, int productId);
    }

}
