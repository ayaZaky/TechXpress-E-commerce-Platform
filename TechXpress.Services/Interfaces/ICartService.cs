using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;

namespace TechXpress.Services.Interfaces
{

    public interface ICartService
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
