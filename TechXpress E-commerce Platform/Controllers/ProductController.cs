using Microsoft.AspNetCore.Mvc;
using TechXpress.Data.Entities;
using TechXpress.Services.Interfaces;
using X.PagedList;
using X.PagedList;
using X.PagedList.Extensions;    
using TechXpress.Data.Repositories.Interfaces;

namespace TechXpress_E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;


        public ProductController(IProductService productService, IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        // Get all products
        public async Task<IActionResult> Index(
            string? searchTerm,
            decimal? minPrice,
            decimal? maxPrice,
            string? brand,
            double? minRating,
            bool? isAvailable,
            string? sortBy,
            bool ascending = true,
            int? categoryId = null,
            int page = 1,
            int pageSize = 6)
        {
            var products = await _productService.FilterProductsAsync(
                searchTerm, minPrice, maxPrice, brand, minRating,
                isAvailable, sortBy, ascending, categoryId);

            var categories = await _unitOfWork.Category.GetAllAsync();
            var allProducts = await _productService.GetAllProductsWithImagesAndCategoriesAsync();

            var categoryProductCounts = categories.ToDictionary(
                category => category.Id,
                category => allProducts.Count(p => p.CategoryId == category.Id)
            );

            ViewBag.Categories = categories;
            ViewBag.CategoryProductCounts = categoryProductCounts;

            return View(products.ToPagedList(page, pageSize));
        }

        // Get product by ID
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductWithDetailsAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // Get products by category id
        [HttpGet]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            if (products == null) return NotFound();
            return View(products);
        }

        // Create Product
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _unitOfWork.Category.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _unitOfWork.Category.GetAllAsync();
            return View(product);
        }

        // Edit Product
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductWithDetailsAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _unitOfWork.Category.GetAllAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _unitOfWork.Category.GetAllAsync();
            return View(product);
        }

        // Delete Product
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductWithDetailsAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetProductWithDetailsAsync(id);
            if (product == null) return NotFound();

            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Featured Products
        public async Task<IActionResult> Featured()
        {
            var products = await _productService.GetFeaturedProductsAsync();
            return View(products);
        }

        // Filter Products
        public async Task<IActionResult> Filter(
            string? searchTerm,
            decimal? minPrice,
            decimal? maxPrice,
            string? brand,
            double? minRating,
            bool? isAvailable,
            string? sortBy,
            bool ascending = true,
            int? categoryId = null)
        {
            var products = await _productService.FilterProductsAsync(
                searchTerm, minPrice, maxPrice, brand, minRating,
                isAvailable, sortBy, ascending, categoryId);

            ViewBag.Categories = await _unitOfWork.Category.GetAllAsync();
            return View(products);
        }

        // Products by Price Range
        public async Task<IActionResult> PriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return View(products);
        }

        // Products by Brand
        public async Task<IActionResult> Brand(string brand)
        {
            var products = await _productService.GetProductsByBrandAsync(brand);
            return View(products);
        }

        // Top Rated Products
        public async Task<IActionResult> TopRated(double minRating = 4.0)
        {
            var products = await _productService.GetTopRatedProductsAsync(minRating);
            return View(products);
        }

        // Search Products
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;
            var products = await _productService.SearchProductsAsync(searchTerm);
            return View(products);
        }

        // New Arrivals
        public async Task<IActionResult> NewArrivals(int days = 30)
        {
            var products = await _productService.GetNewArrivalsAsync(days);
            return View(products);
        }
    }
}
