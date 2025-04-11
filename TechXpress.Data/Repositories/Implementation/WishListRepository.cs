using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.AppContext;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Interfaces;

namespace TechXpress.Data.Repositories.Implementation
{
    public class WishListRepository : Repository<WishListItem>, IWishListRepository
    {
        public WishListRepository(AppDbContext context) : base(context) { }

       
        public async Task<IEnumerable<WishListItem>> GetWishListItemsAsync(string userId)
        {
            return await _context.WishListItems
                .Include(w => w.Product)
                    .ThenInclude(w => w.Images)
                .Include(w => w.Product.Category)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<WishListItem> AddToWishListAsync(int productId, string userId)
        {
            var existingItem = await _context.WishListItems
                .FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);

            if (existingItem != null)
            {
                return existingItem;  
            }

            var wishListItem = new WishListItem
            {
                ProductId = productId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.WishListItems.AddAsync(wishListItem);
            return wishListItem;
        }
       
        public async Task RemoveFromWishListAsync(int wishListItemId, string userId)
        {
            var wishListItem = await _context.WishListItems
                .FirstOrDefaultAsync(w => w.Id == wishListItemId && w.UserId == userId);

            if (wishListItem != null)
            {
                _context.WishListItems.Remove(wishListItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsInWishListAsync(int productId, string userId)
        {
            return await _context.WishListItems
                .AnyAsync(w => w.ProductId == productId && w.UserId == userId);
        }
    }
}

