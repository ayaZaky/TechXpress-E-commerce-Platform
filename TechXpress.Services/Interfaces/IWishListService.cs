using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;

namespace TechXpress.Services.Interfaces
{
    public interface IWishListService
    {
        Task<IEnumerable<WishListItem>> GetWishListItemsAsync(string userId);
        Task<WishListItem> AddToWishListAsync(int productId, string userId);
        Task RemoveFromWishListAsync(int wishListItemId, string userId);
        Task<bool> IsInWishListAsync(int productId, string userId);
    }
}
