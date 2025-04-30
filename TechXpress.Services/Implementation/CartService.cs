using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Services.Interfaces;

namespace TechXpress.Services.Implementation
{
        public class CartService : ICartService
        {
            private readonly IUnitOfWork _unitOfWork;

            public CartService(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
            {
                return await _unitOfWork.Cart.GetCartItemsAsync(userId);
            }

            public async Task<CartItem> AddToCartAsync(int productId, string userId, int quantity)
            {
                var cartItem = await _unitOfWork.Cart.AddToCartAsync(productId, userId, quantity);
                await _unitOfWork.CompleteAsync();
                return cartItem;
            }

            public async Task<CartItem> UpdateOrAddToCartAsync(int productId, string userId, int quantity)
            {
                var cartItem = await _unitOfWork.Cart.UpdateOrAddToCartAsync(productId, userId, quantity);
                await _unitOfWork.CompleteAsync();
                return cartItem;
            }

            public async Task RemoveFromCartAsync(int cartItemId, string userId)
            {
                await _unitOfWork.Cart.RemoveFromCartAsync(cartItemId, userId);
                await _unitOfWork.CompleteAsync();
            }

            public async Task UpdateQuantityAsync(int cartItemId, string userId, int quantity)
            {
                await _unitOfWork.Cart.UpdateQuantityAsync(cartItemId, userId, quantity);
                await _unitOfWork.CompleteAsync();
            }

            public async Task<int> GetCartItemCountAsync(string userId)
            {
                return await _unitOfWork.Cart.GetCartItemCountAsync(userId);
            }

            public async Task<CartItem> GetCartItemAsync(string userId, int productId)
            {
                return await _unitOfWork.Cart.GetCartItemAsync(userId, productId);
            }
        }
    }

