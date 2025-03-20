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
    public class ProductService:IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Product.GetAllAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.Product.GetByIdAsync(id);
        }
        public async Task<bool> CreateProductAsync(Product product)
        {
            await _unitOfWork.Product.AddAsync(product);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            _unitOfWork.Product.Update(product);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null) return false;
            _unitOfWork.Product.Remove(product);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        public Task<int> GetProductCountAsync()
        {
            return _unitOfWork.Product.CountAsync(p => true);
        }
        public Task<int> GetProductCountByCategoryAsync(int categoryId)
        {
            return _unitOfWork.Product.CountAsync(p => p.CategoryId == categoryId);
        }
        public Task<int> GetProductCountByBrandAsync(string brand)
        {
            return _unitOfWork.Product.CountAsync(p => p.Brand == brand);
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithImagesAndCategoriesAsync()
        {
            return await _unitOfWork.Product.GetAllProductsWithImagesAndCategoriesAsync();
        }

        public async Task<Product?> GetProductWithImagesAsync(int productId)
        {
            return await _unitOfWork.Product.GetProductWithImagesAsync(productId);
        }

        public async Task<Product?> GetProductWithDetailsAsync(int productId)
        {
            return await _unitOfWork.Product.GetProductWithDetailsAsync(productId);
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(productId);
            if (product == null) return false;

            product.StockQuantity = quantity;
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Product.GetProductsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            return await _unitOfWork.Product.GetFeaturedProductsAsync();
        }

        public async Task<IEnumerable<Product>> FilterProductsAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brand = null,
            double? minRating = null,
            bool? isAvailable = null,
            string? sortBy = null,
            bool ascending = true,
            int? categoryId = null)
        {
            return await _unitOfWork.Product.FilterProductsAsync(searchTerm, minPrice, maxPrice, brand, minRating, isAvailable, sortBy, ascending, categoryId);
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _unitOfWork.Product.GetProductsByPriceRangeAsync(minPrice, maxPrice);
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand)
        {
            return await _unitOfWork.Product.GetProductsByBrandAsync(brand);
        }

        public async Task<IEnumerable<Product>> GetTopRatedProductsAsync(double minRating = 4.0)
        {
            return await _unitOfWork.Product.GetTopRatedProductsAsync(minRating);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _unitOfWork.Product.SearchProductsAsync(searchTerm);
        }

        public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int days = 30)
        {
            return await _unitOfWork.Product.GetNewArrivalsAsync(days);
        }
    }
}
