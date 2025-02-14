//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TechXpress_E_commerce.Controllers;
//using TechXpress_E_commerce.Models.AppDbContext;
//using TechXpress_E_commerce.Models;
//using TechXpress_E_commerce.Repositories;
//using Xunit;

//namespace TechXpress_E_commerce.Tests
//{
//    public class OrderItemControllerTests
//    {
//        private readonly OrderItemController _controller;
//        private readonly AppDbContext _context;
//        private readonly Repository<OrderItem> _orderItemRepository;

//        public OrderItemControllerTests()
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            _context = new AppDbContext(options);
//            _orderItemRepository = new Repository<OrderItem>(_context);
//            SeedDatabase();

//            _controller = new OrderItemController(_orderItemRepository);
//        }

//        private void SeedDatabase()
//        {
//            _orderItemRepository.Add(new OrderItem { Id = 1, Quantity = 2 });
//            _orderItemRepository.Add(new OrderItem { Id = 2, Quantity = 5 });
//            _orderItemRepository.SaveChanges();
//        }

//        [Fact]
//        public void Index_ReturnsAViewResult_WithAListOfOrderItems()
//        {
//            // Act
//            var result = _controller.Index() as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            var model = Assert.IsAssignableFrom<IEnumerable<OrderItem>>(result.Model);
//            Assert.Equal(2, model.Count());
//        }

//        [Fact]
//        public void Details_OrderItemExists_ReturnsView()
//        {
//            // Act
//            var result = _controller.Details(1) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            var model = Assert.IsType<OrderItem>(result.Model);
//            Assert.Equal(2, model.Quantity);
//        }

//        [Fact]
//        public void Details_OrderItemDoesNotExist_ReturnsNotFound()
//        {
//            // Act
//            var result = _controller.Details(100) as NotFoundResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(404, result.StatusCode);
//        }

//        [Fact]
//        public void Create_InvalidModelState_ReturnsView()
//        {
//            // Arrange
//            _controller.ModelState.AddModelError("Quantity", "Quantity is required");
//            var orderItem = new OrderItem();

//            // Act
//            var result = _controller.Create(orderItem) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.IsType<OrderItem>(result.Model);
//        }

//        [Fact]
//        public void Create_ValidModelState_RedirectsToIndex()
//        {
//            // Arrange
//            var orderItem = new OrderItem { Quantity = 3 };

//            // Act
//            var result = _controller.Create(orderItem) as RedirectToActionResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Index", result.ActionName);
//        }

//        [Fact]
//        public void Edit_OrderItemExists_ReturnsView()
//        {
//            // Act
//            var result = _controller.Edit(1) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            var model = Assert.IsType<OrderItem>(result.Model);
//            Assert.Equal(2, model.Quantity);
//        }

//        [Fact]
//        public void Edit_OrderItemDoesNotExist_ReturnsNotFound()
//        {
//            // Act
//            var result = _controller.Edit(100) as NotFoundResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(404, result.StatusCode);
//        }

//        [Fact]
//        public void Edit_InvalidModelState_ReturnsView()
//        {
//            // Arrange
//            var orderItem = new OrderItem { Id = 1, Quantity = 0 };
//            _controller.ModelState.AddModelError("Quantity", "Required");

//            // Act
//            var result = _controller.Edit(1, orderItem) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.IsType<OrderItem>(result.Model);
//        }

//        [Fact]
//        public void Edit_ValidModelState_RedirectsToIndex()
//        {
//            // Arrange
//            var orderItem = _context.OrderItems.FirstOrDefault(oi => oi.Id == 1);
//            Assert.NotNull(orderItem);

//            orderItem.Quantity = 10;

//            // Act
//            var result = _controller.Edit(orderItem.Id, orderItem) as RedirectToActionResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Index", result.ActionName);

//            var updatedOrderItem = _context.OrderItems.Find(orderItem.Id);
//            Assert.NotNull(updatedOrderItem);
//            Assert.Equal(10, updatedOrderItem.Quantity);
//        }

//        [Fact]
//        public void Delete_OrderItemExists_ReturnsView()
//        {
//            // Act
//            var result = _controller.Delete(1) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            var model = Assert.IsType<OrderItem>(result.Model);
//            Assert.Equal(2, model.Quantity);
//        }

//        [Fact]
//        public void Delete_OrderItemDoesNotExist_ReturnsNotFound()
//        {
//            // Act
//            var result = _controller.Delete(100) as NotFoundResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(404, result.StatusCode);
//        }

//        [Fact]
//        public void DeleteConfirmed_OrderItemExists_RedirectsToIndex()
//        {
//            // Act
//            var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Index", result.ActionName);
//        }

//        [Fact]
//        public void DeleteConfirmed_OrderItemDoesNotExist_ReturnsNotFound()
//        {
//            // Act
//            var result = _controller.DeleteConfirmed(100) as NotFoundResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(404, result.StatusCode);
//        }
//    }
//}
