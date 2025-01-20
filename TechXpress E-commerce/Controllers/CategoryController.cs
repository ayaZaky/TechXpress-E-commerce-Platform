using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Repositories;

namespace TechXpress_E_commerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        } 
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();  
            return View(categories);
        } 
        public IActionResult Create()
        {
            return View();
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(category);  
                _categoryRepository.SaveChanges(); 
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        } 
        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetById(id);  
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category); 
                _categoryRepository.SaveChanges();  
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);  
            if (category == null)
            {
                return NotFound();
            }

            _categoryRepository.Delete(category); 
            _categoryRepository.SaveChanges(); 
            return RedirectToAction(nameof(Index));
        }
    }
}
