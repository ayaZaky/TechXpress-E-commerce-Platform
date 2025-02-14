using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Repositories;

namespace TechXpress_E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _p_repository;
        private readonly IRepository<Category> _c_repository;
        private readonly IProductRepository _productRepository;

        public ProductController(IRepository<Product> p_repository, IRepository<Category> c_repository, IProductRepository productRepository)
        {
            _p_repository = p_repository;
            _c_repository = c_repository;
            _productRepository = productRepository;
        }

        // Get all products
        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsWithImagesAndCategoriesAsync();
            return View(products);
        }

        [HttpGet]
        // Get product by ID
        public async Task<IActionResult>Details(int id)
        {
            var product = await _productRepository.GetProductWithDetailsAsync(id);
            if (product == null)
            { return NotFound();
            }    
            return View(product);
        }
      

        // Get products by category id
        [HttpGet]

        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var products = await _p_repository.GetByIdAsync(categoryId);
            if (products == null)
                return NotFound();
            return View(products);
            
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _c_repository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _p_repository.AddAsync(product);
                await _p_repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _c_repository.GetAllAsync();
            return View(product);
        }

        // Edit
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _p_repository.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _c_repository.GetAllAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _p_repository.Update(product);
                await _p_repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _c_repository.GetAllAsync();
            return View(product);
        }

        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _p_repository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _p_repository.GetByIdAsync(id);
            if (product == null) return NotFound();

            _p_repository.Remove(product);
            await _p_repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> QuickView(int id)
        {
            var product = await _p_repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return PartialView("_QuickView", product); // PartialView to display a modal window
        }
    }
}
