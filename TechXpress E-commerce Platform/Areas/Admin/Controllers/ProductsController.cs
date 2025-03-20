using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories;    
using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Services.Interfaces;

namespace TechXpress.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;


    public ProductsController(IProductService productService, IUnitOfWork unitOfWork)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
    }


    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsWithImagesAndCategoriesAsync();  
        var categories = await _unitOfWork.Category.GetAllAsync();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();
        return View(products);
      
    }
    //[HttpGet]
    //public async Task<IActionResult> Create()
    //{
    //    var categories = await _unitOfWork.Category.GetAllAsync();

    //    ViewBag.Categories = categories.Select(c => new SelectListItem
    //    {
    //        Value = c.Id.ToString(),
    //        Text = c.Name
    //    }).ToList();
    //    return PartialView("_ProductModals");
    //}
    [HttpPost]
    [ValidateAntiForgeryToken]  
    public async Task<IActionResult> Create(Product product, List<IFormFile>? images)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateProductAsync(product);

            // لو فيه صور مرفوعة
            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    // هنا المفروض تحفظ الصورة في wwwroot/uploads وترجع الـ URL
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var productImage = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = $"/uploads/{fileName}"
                        
                    };
                    await _unitOfWork.ProductImage.AddAsync(productImage);
                }
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        var categories = await _unitOfWork.Category.GetAllAsync();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        return PartialView("_ProductModals", product);
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
    [ValidateAntiForgeryToken]
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

    //// Delete Product
    //[HttpGet]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var product = await _productService.GetProductWithDetailsAsync(id);
    //    if (product == null) return NotFound();
    //    return View(product);
    //}

    //[HttpPost, ActionName("Delete")]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    var product = await _productService.GetProductWithDetailsAsync(id);
    //    if (product == null) return NotFound();

    //    await _productService.DeleteProductAsync(id);
    //    return RedirectToAction(nameof(Index));
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetProductWithDetailsAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        await _productService.DeleteProductAsync(id);   
        return RedirectToAction(nameof(Index));
    }
}