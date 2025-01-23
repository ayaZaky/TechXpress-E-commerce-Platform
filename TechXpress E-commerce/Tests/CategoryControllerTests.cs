using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Controllers;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Repositories;
using Xunit;

namespace TechXpress_E_commerce.Tests
{
    public class CategoryControllerTests
    {
        private readonly CategoryController _controller;
        private readonly AppDbContext _context;
        private readonly Repository<Category> _categoryRepository;

        public CategoryControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _categoryRepository = new Repository<Category>(_context);
            SeedDatabase();

            _controller = new CategoryController(_categoryRepository);
        }

        private void SeedDatabase()
        {
            _categoryRepository.Add(new Category { Id = 1, Name = "Category1" });
            _categoryRepository.Add(new Category { Id = 2, Name = "Category2" });
            _categoryRepository.SaveChanges();
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfCategories()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(result.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Name is required");
            var category = new Category();

            // Act
            var result = _controller.Create(category) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result.Model);
        }

        [Fact]
        public void Create_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var category = new Category { Name = "Category3" };

            // Act
            var result = _controller.Create(category) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_CategoryExists_ReturnsView()
        {
            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Category>(result.Model);
            Assert.Equal("Category1", model.Name);
        }

        [Fact]
        public void Edit_CategoryDoesNotExist_ReturnsNotFound()
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
            var category = new Category { Id = 1, Name = "" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Edit(1, category) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result.Model);
        }

        [Fact]
        public void Edit_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var category = _context.Categories.FirstOrDefault(c => c.Id == 1);
            Assert.NotNull(category);

            category.Name = "UpdatedCategory1";

            // Act
            var result = _controller.Edit(category.Id, category) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var updatedCategory = _context.Categories.Find(category.Id);
            Assert.NotNull(updatedCategory);
            Assert.Equal("UpdatedCategory1", updatedCategory.Name);
        }

        [Fact]
        public void Delete_CategoryExists_RedirectsToIndex()
        {
            // Act
            var result = _controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Delete_CategoryDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Delete(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
