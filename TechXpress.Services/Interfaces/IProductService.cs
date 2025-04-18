﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;

namespace TechXpress.Services.Interfaces
{
    public interface IProductService
    {
        // Basic CRUD operations
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsWithImagesAndCategoriesAsync();
        Task<Product?> GetProductWithImagesAsync(int productId);
        Task<Product?> GetProductWithDetailsAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantity);

        // Category-based queries
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();

        // Filter methods

        Task<IEnumerable<Product>> FilterProductsAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brand = null,
            double? minRating = null,
            bool? isAvailable = null,
            string? sortBy = null,
            bool ascending = true,
            int? categoryId = null);

        // Specialized filter methods
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand);
        Task<IEnumerable<Product>> GetTopRatedProductsAsync(double minRating = 4.0);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Product>> GetNewArrivalsAsync(int days = 30);
    }
}
