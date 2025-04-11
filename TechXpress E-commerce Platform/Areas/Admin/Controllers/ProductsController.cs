using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories;    
using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Services.Interfaces;
using TechXpress_E_commerce_Platform.Areas.Admin.ViewModels;

namespace TechXpress.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
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
 
    public async Task<IActionResult> Create(ProductViewModel model)
    {
       

        if (!ModelState.IsValid)
        {
            var errorMessages = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
            return Json(new { success = false, message = errorMessages });
        } 
        try
        {
            // إنشاء المنتج
            var product = new Product
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                Brand = model.Brand,
                Rating = model.Rating,
                Specifications = model.Specifications,
                IsAvailable = model.IsAvailable,
                IsFeatured = model.IsFeatured,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _unitOfWork.Product.AddAsync(product);
            await _unitOfWork.Product.SaveChangesAsync();

            var productImages = new List<ProductImage>();

            if (model.UploadedImages != null && model.UploadedImages.Any())
            {
                foreach (var file in model.UploadedImages)
                {
                    if (file.Length > 0)
                    {     
                        var fileName = "product-" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                         
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", fileName);  
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        productImages.Add(new ProductImage
                        {
                            ProductId = product.Id,
                            ImageUrl = "/images/products/" + fileName  
                        });
                              

                    }
                }
            }
            var uploadedImagePaths = productImages
            .Where(img => img.ImageUrl.StartsWith("/images/products/"))
            .Select(img => img.ImageUrl)
            .ToHashSet();

            if (model.ImageUrls != null)
            {
                foreach (var url in model.ImageUrls)
                {
                    var trimmedUrl = url.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedUrl) && !uploadedImagePaths.Contains(trimmedUrl))
                    {
                        productImages.Add(new ProductImage
                        {
                            ProductId = product.Id,
                            ImageUrl = trimmedUrl
                        });
                    }
                }
            }
            await _unitOfWork.ProductImage.AddRangeAsync(productImages);
            await _unitOfWork.ProductImage.SaveChangesAsync();

            return Json(new { success = true, message = "Product Added Successfully!" });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "An error occurred while saving the product. Please try again." });
        }
    }


    // Edit Product
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var model = new ProductViewModel
        {
            Name = product.Name,
            CategoryId = product.CategoryId,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Brand = product.Brand,
            Rating = product.Rating,
            Specifications = product.Specifications,
            IsAvailable = product.IsAvailable,
            IsFeatured = product.IsFeatured,
            ImageUrls = product.Images.Select(p => p.ImageUrl).ToList()

        };
        var categories = await _unitOfWork.Category.GetAllAsync();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();
        // إرسال الـ model مش الـ product
        return PartialView("_EditProductPartial", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel model)
    {
      

        if (!ModelState.IsValid)
        {
            var errorMessages = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
            return Json(new { success = false, message = errorMessages });
        }

        try
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id); // الحصول على المنتج
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found!" });
            }

            // تحديث خصائص المنتج
            product.Name = model.Name;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;  
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;
            product.Brand = model.Brand;
            product.Rating = model.Rating;
            product.Specifications = model.Specifications;
            product.IsAvailable = model.IsAvailable;
            product.IsFeatured = model.IsFeatured;  
            product.UpdatedAt = DateTime.Now;
             
            if (model.UploadedImages != null && model.UploadedImages.Any())
            {
                foreach (var file in model.UploadedImages)
                {
                    if (file.Length > 0)
                    {
                         
                        var fileName = "product-" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        // تحديد المسار داخل مجلد wwwroot/images/products
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", fileName);

                        // حفظ الصورة في المسار المحدد
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // إضافة صورة المنتج إلى قاعدة البيانات
                        var image = new ProductImage
                        {
                            ProductId = product.Id,
                            ImageUrl = "/images/products/" + fileName // حفظ الرابط الخاص بالصورة
                        };
                        await _unitOfWork.ProductImage.AddAsync(image);
                    }
                }
            }

            // 2. تحديث الصور من الروابط (إذا تم تعديلها)
            if (model.ImageUrls != null)
            {
                // جلب الصور القديمة من المنتج مباشرة
                var existingImages = product.Images;  // نستخدم Images من Product

                // حذف الصور القديمة التي تم تحديثها (لو حابب تحذف الصور القديمة أولًا)
                _unitOfWork.ProductImage.RemoveRange(existingImages);

                // إضافة الصور الجديدة من الروابط
                foreach (var url in model.ImageUrls)
                {
                    if (!string.IsNullOrWhiteSpace(url) && !existingImages.Any(img => img.ImageUrl == url.Trim()))
                    {
                        var image = new ProductImage
                        {
                            ProductId = product.Id,
                            ImageUrl = url.Trim()
                        };
                        await _unitOfWork.ProductImage.AddAsync(image);
                    }
                }
            }

            await _unitOfWork.CompleteAsync(); // حفظ التحديثات في قاعدة البيانات
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));

        }
        catch (Exception)
        {
            return PartialView("_EditCategoryPartial",  model);
        }
    }


    //[HttpGet]
    //public async Task<IActionResult> Edit(int id)
    //{
    //    var product = await _productService.GetProductWithDetailsAsync(id);
    //    if (product == null) return NotFound();

    //    ViewBag.Categories = await _unitOfWork.Category.GetAllAsync();
    //    return View(product);
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, Product product)
    //{
    //    if (id != product.Id) return BadRequest();
    //    if (ModelState.IsValid)
    //    {
    //        await _productService.UpdateProductAsync(product);
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewBag.Categories = await _unitOfWork.Category.GetAllAsync();
    //    return View(product);
    //}


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id);
        if (product == null)
        {
            return Json(new { success = false, message = "Product not found." });
        }

        _unitOfWork.Product.Remove(product);
        await _unitOfWork.Product.SaveChangesAsync();

        return Json(new { success = true, message = "Product deleted successfully." });
    }
    
}