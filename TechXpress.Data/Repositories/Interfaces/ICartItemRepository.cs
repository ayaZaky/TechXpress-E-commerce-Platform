using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Data.Entities;

namespace TechXpress.Data.Repositories.Interfaces
{
    public interface ICartItemRepository:IRepository<CartItem>
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
