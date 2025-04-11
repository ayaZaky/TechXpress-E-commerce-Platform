using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Entities;
using TechXpress.Data.AppContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using TechXpress.Data.Repositories.Implementation;
using TechXpress.Data.Repositories.Interfaces;

namespace TechXpress_E_commerce.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AppDbContext context) : base(context) { }      

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Images)
                .Include(ci => ci.Product.Category)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartItem> AddToCartAsync(int productId, string userId, int quantity)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                existingItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.UtcNow
                };
                _context.CartItems.Add(existingItem);
            }

            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task<CartItem> UpdateOrAddToCartAsync(int productId, string userId, int quantity)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                existingItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.UtcNow
                };
                _context.CartItems.Add(existingItem);
            }

            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task RemoveFromCartAsync(int cartItemId, string userId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateQuantityAsync(int cartItemId, string userId, int quantity)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task<CartItem> GetCartItemAsync(string userId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        }
    }
}
