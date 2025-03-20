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
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishListService(IUnitOfWork unitOfWork)
        {                                   
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WishListItem>> GetWishListItemsAsync(string userId)
        {
            return await  _unitOfWork.WishList.GetWishListItemsAsync(userId);
        }

        public async Task<WishListItem> AddToWishListAsync(int productId, string userId)
        {
            bool isInWishList = await _unitOfWork.WishList.IsInWishListAsync(productId, userId);
            if (isInWishList)
            {
                return null; 
            }

            var wishListItem = await _unitOfWork.WishList.AddToWishListAsync(productId, userId);
            await _unitOfWork.CompleteAsync();
            return wishListItem;
        }

        public async Task RemoveFromWishListAsync(int wishListItemId, string userId)
        {
            await _unitOfWork.WishList.RemoveFromWishListAsync(wishListItemId, userId);
            await _unitOfWork.CompleteAsync();
 
        }

        public async Task<bool> IsInWishListAsync(int productId, string userId)
        {
            return await _unitOfWork.WishList.IsInWishListAsync(productId, userId);
        }

  
    }
}
