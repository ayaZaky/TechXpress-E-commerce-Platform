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

        public ProductController(IRepository<Product> p_repository , IRepository<Category> c_repository,IProductRepository productRepository)
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
        public IActionResult Details(int id)
        {
            var product =  _p_repository.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        // Get products by category id
        [HttpGet]
        public IActionResult GetProductsByCategoryId(int categoryId)
        {
            var products = _p_repository.GetAll()
                                 .AsQueryable()  
                                .Include(p => p.Category)  
                                .Where(p => p.CategoryId == categoryId)
                                .ToList();  

            if (!products.Any())
                return NotFound();

            return View(products);
        }


        // Create
        [HttpGet]
        public IActionResult Create()
        { 
            ViewBag.Categories = _c_repository.GetAll(); ;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _p_repository.Add(product) ;
                _p_repository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _c_repository.GetAll(); 
            return View(product);
        }

        //Edit
        public IActionResult Edit(int id)
        {
            var product = _p_repository.GetById(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _c_repository.GetAll();
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _p_repository.Update(product);
                _p_repository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories =_c_repository.GetAll();
            return View(product);
        }
        //Delete
        public IActionResult Delete(int id)
        {
            var product = _p_repository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _p_repository.GetById(id);
            if (product == null) return NotFound();

            _p_repository.Delete(product);
            _p_repository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
