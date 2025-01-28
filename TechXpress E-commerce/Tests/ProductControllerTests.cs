using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Controllers;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Repositories;
using Xunit;

namespace TechXpress_E_commerce.Tests
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly  AppDbContext _context;
        private readonly Repository<Product> _p_repository;
        private readonly Repository<Category> _c_repository;


        //public ProductControllerTests()
        //{
        //    var options = new DbContextOptionsBuilder<AppDbContext>()
        //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        //        .Options;

        //    _context = new AppDbContext(options);
        //    _p_repository =new Repository<Product>(_context);
        //    _c_repository = new Repository<Category>(_context);
        //    SeedDatabase();

        //    _controller = new ProductController(_p_repository, _c_repository);
        //}
        private void SeedDatabase()
        {
            var category = new Category {  Id = 1, Name = "Category1" };
            _c_repository.Add(category); 
            _c_repository.SaveChanges();
            _p_repository.Add(new Product {  Id = 1, Name = "Product1",  CategoryId = 1 });
            _p_repository.Add(new Product { Id = 2, Name = "Product2", CategoryId = 1 });
            _p_repository.SaveChanges();
        }
        //[Fact]
        //public void Index_ReturnsAViewResult_WithAListOfProducts()
        //{
        //    // Act
        //    var result = _controller.Index() as ViewResult; 
        //    // Assert
        //    Assert.NotNull(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<Product>>(result.Model);
        //    Assert.Equal(2, model.Count());
        //}
        [Fact]
        public void Details_ProductExists_ReturnsView()
        {
            // Act
            var result = _controller.Details(1) as ViewResult;
            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Product>(result.Model);
            Assert.Equal("Product1", model.Name);
        }
        [Fact]
        public void Details_ProductDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Details(100) as NotFoundResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
        [Fact]
        public void GetProductsByCategoryId_CategoryExists_ReturnsView()
        {
            // Act
            var result = _controller.GetProductsByCategoryId(1) as ViewResult;
            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(result.Model);
            Assert.Equal(2, model.Count());
        }
        [Fact]
        public void GetProductsByCategoryId_CategoryDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetProductsByCategoryId(100) as NotFoundResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Name is required");
            var product = new Product();
            // Act
            var result = _controller.Create(product) as ViewResult;
            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result.Model);
        }
        [Fact]
        public void Create_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var product = new Product { Name = "Product3", CategoryId = 1 };
            // Act
            var result = _controller.Create(product) as RedirectToActionResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
        [Fact]
        public void Edit_ProductExists_ReturnsView()
        {
            // Act
            var result = _controller.Edit(1) as ViewResult;
            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Product>(result.Model);
            Assert.Equal("Product1", model.Name);
        }
        [Fact]
        public void Edit_ProductDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Edit(100) as NotFoundResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
        [Fact]

        public void Edit_InvalidModelState_ReturnsView()
        {
            // Arrange
            var product = new Product {Id = 1, Name = "" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result =  _controller.Edit(1, product) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result.Model);
            
        }
        [Fact]

        public void Edit_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var product = _context.Products.FirstOrDefault(p => p.Id == 1);
            Assert.NotNull(product);

            product.Name = "UpdatedProduct1";

            // Act
            var result =  _controller.Edit(product.Id, product) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var updatedProduct = _context.Products.Find(product.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal("UpdatedProduct1", updatedProduct.Name);
        }
       
        [Fact]
        public void Delete_ProductExists_ReturnsView()
        {
            // Act
            var result = _controller.Delete(1) as ViewResult;
            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Product>(result.Model);
            Assert.Equal("Product1", model.Name);
        }
        [Fact]
        public void Delete_ProductDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Delete(100) as NotFoundResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
        [Fact]
        public void DeleteConfirmed_ProductExists_RedirectsToIndex()
        {
            // Act
            var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
        [Fact]
        public void DeleteConfirmed_ProductDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.DeleteConfirmed(100) as NotFoundResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
        

    }
}
