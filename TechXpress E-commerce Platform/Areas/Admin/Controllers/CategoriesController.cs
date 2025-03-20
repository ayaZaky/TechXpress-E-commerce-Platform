using Microsoft.AspNetCore.Mvc;
using TechXpress.Data.Repositories;
using TechXpress.Data.Entities;
using TechXpress.Web.Areas.Admin.Controllers;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Services.Interfaces;
using TechXpress.Services.Implementation;

namespace TechXpress.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();

           
            var allProducts = await _unitOfWork.Product.GetAllProductsWithImagesAndCategoriesAsync();

            var categoryProductCounts = categories.ToDictionary(
                category => category.Id,
                category => allProducts.Count(p => p.CategoryId == category.Id)
            );
              
            ViewBag.CategoryProductCounts = categoryProductCounts;
                         
            return View(categories);
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.AddAsync(category);
                await _unitOfWork.Category.SaveChangesAsync();
                TempData["Success"] = "Category Added Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_AddCategoryPartial");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
        return PartialView("_EditCategoryPartial", category);
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                await _unitOfWork.Category.SaveChangesAsync();
                TempData["Success"] = "Category Updated Successfully!";
                return RedirectToAction(nameof(Index));
        }
        return PartialView("_EditCategoryPartial", category);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id);
        if (category == null)
        {
            return Json(new { success = false, message = "Category not found." });
        }

        _unitOfWork.Category.Remove(category);
        await _unitOfWork.Category.SaveChangesAsync();

        return Json(new { success = true, message = "Category deleted successfully." });
    }


    //[HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        var category = await _unitOfWork.Category.GetByIdAsync(id);
    //        if (category == null)
    //        {
    //            return NotFound();
    //        }

    //        _unitOfWork.Category.Remove(category);
    //        await _unitOfWork.Category.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }
}
