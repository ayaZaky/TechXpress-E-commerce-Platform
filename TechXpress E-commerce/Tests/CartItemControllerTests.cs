using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Controllers;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Repositories;
using Xunit;

namespace TechXpress_E_commerce.Tests
{
    public class CartItemControllerTests
    {
        private readonly CartItemController _controller;
        private readonly AppDbContext _context;
        private readonly Repository<CartItem> _cartItemRepository;

        public CartItemControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _cartItemRepository = new Repository<CartItem>(_context);
            SeedDatabase();

            _controller = new CartItemController(_cartItemRepository);
        }

        private void SeedDatabase()
        {
            _cartItemRepository.Add(new CartItem { Id = 1, Quantity = 2, CreatedAt = DateTime.Now });
            _cartItemRepository.Add(new CartItem { Id = 2, Quantity = 5, CreatedAt = DateTime.Now });
            _cartItemRepository.SaveChanges();
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfCartItems()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CartItem>>(result.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Details_CartItemExists_ReturnsView()
        {
            // Act
            var result = _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<CartItem>(result.Model);
            Assert.Equal(2, model.Quantity);
        }

        [Fact]
        public void Details_CartItemDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Details(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Quantity", "Quantity is required");
            var cartItem = new CartItem();

            // Act
            var result = _controller.Create(cartItem) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CartItem>(result.Model);
        }

        [Fact]
        public void Create_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var cartItem = new CartItem { Quantity = 3, CreatedAt = DateTime.Now };

            // Act
            var result = _controller.Create(cartItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_CartItemExists_ReturnsView()
        {
            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<CartItem>(result.Model);
            Assert.Equal(2, model.Quantity);
        }

        [Fact]
        public void Edit_CartItemDoesNotExist_ReturnsNotFound()
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
            var cartItem = new CartItem { Id = 1, Quantity = 0 };
            _controller.ModelState.AddModelError("Quantity", "Quantity is required");

            // Act
            var result = _controller.Edit(1, cartItem) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CartItem>(result.Model);
        }

        [Fact]
        public void Edit_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Id == 1);
            Assert.NotNull(cartItem);

            cartItem.Quantity = 10;

            // Act
            var result = _controller.Edit(cartItem.Id, cartItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var updatedCartItem = _context.CartItems.Find(cartItem.Id);
            Assert.NotNull(updatedCartItem);
            Assert.Equal(10, updatedCartItem.Quantity);
        }

        [Fact]
        public void Delete_CartItemExists_ReturnsView()
        {
            // Act
            var result = _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<CartItem>(result.Model);
            Assert.Equal(2, model.Quantity);
        }

        [Fact]
        public void Delete_CartItemDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Delete(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void DeleteConfirmed_CartItemExists_RedirectsToIndex()
        {
            // Act
            var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void DeleteConfirmed_CartItemDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.DeleteConfirmed(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
