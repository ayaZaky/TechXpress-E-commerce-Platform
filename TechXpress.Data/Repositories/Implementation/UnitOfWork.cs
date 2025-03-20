using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.AppContext;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress_E_commerce.Repositories;


namespace TechXpress.Data.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IProductRepository Product { get; private set; }
        public IRepository<Category> Category{ get; private set; }
        public ICartItemRepository Cart { get; private set; }
        public IWishListRepository WishList { get; private set; }
        public IRepository<Order> Order { get; private set; }
        public IRepository<OrderItem> OrderItem { get; private set; }
        public IRepository<ProductImage> ProductImage { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Product = new ProductRepository(context);
            Category = new Repository<Category>(context);
            Cart = new CartItemRepository(context);
            WishList = new WishListRepository(context);
            Order = new Repository<Order>(context);
            OrderItem = new Repository<OrderItem>(context);
            ProductImage = new Repository<ProductImage>(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}