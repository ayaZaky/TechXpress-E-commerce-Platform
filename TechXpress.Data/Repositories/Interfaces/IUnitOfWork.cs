using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;

namespace TechXpress.Data.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IProductRepository Product { get; }   
        IRepository<Category> Category { get; }
        ICartItemRepository Cart { get; }
        IWishListRepository WishList { get; }
        IRepository<Order> Order { get; }
        IRepository<OrderItem> OrderItem { get; }
        IRepository<ProductImage> ProductImage { get; }
        Task<int> CompleteAsync();

    }
}
 