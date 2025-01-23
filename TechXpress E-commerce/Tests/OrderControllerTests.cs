using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Controllers;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Repositories;
using Xunit;

namespace TechXpress_E_commerce.Tests
{
    public class OrderControllerTests
    {
        private readonly OrderController _controller;
        private readonly AppDbContext _context;
        private readonly Repository<Order> _orderRepository;

        public OrderControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _orderRepository = new Repository<Order>(_context);
            SeedDatabase();

            _controller = new OrderController(_orderRepository);
        }

        private void SeedDatabase()
        {
            _orderRepository.Add(new Order
            {
                Id = 1,
                UserId = 1,
                Subtotal = 50,
                Tax = 10,
                ShippingFee = 5,
                Total = 65,
                ShippingAddress = "123 Main St",
                BillingAddress = "123 Main St",
                 
            });

            _orderRepository.Add(new Order
            {
                Id = 2,
                UserId = 2,
                Subtotal = 150,
                Tax = 30,
                ShippingFee = 10,
                Total = 190,
                ShippingAddress = "456 Other St",
                BillingAddress = "456 Other St",
              
            });

            _orderRepository.SaveChanges();
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfOrders()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Order>>(result.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Details_OrderExists_ReturnsView()
        {
            // Act
            var result = _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Order>(result.Model);
            Assert.Equal(65, model.Total); // Validate the correct total
        }

        [Fact]
        public void Details_OrderDoesNotExist_ReturnsNotFound()
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
            _controller.ModelState.AddModelError("Total", "Total is required");
            var order = new Order();

            // Act
            var result = _controller.Create(order) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result.Model);
        }

        [Fact]
        public void Create_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var order = new Order
            {
                UserId = 3,
                Subtotal = 200,
                Tax = 40,
                ShippingFee = 15,
                Total = 255,
                ShippingAddress = "789 New St",
                BillingAddress = "789 New St",
                 
            };

            // Act
            var result = _controller.Create(order) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_OrderExists_ReturnsView()
        {
            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Order>(result.Model);
            Assert.Equal(65, model.Total); // Validate the correct total
        }

        [Fact]
        public void Edit_OrderDoesNotExist_ReturnsNotFound()
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
            var order = new Order { Id = 1, Total = 0 };
            _controller.ModelState.AddModelError("Total", "Required");

            // Act
            var result = _controller.Edit(1, order) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result.Model);
        }

        [Fact]
        public void Edit_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var order = _context.Orders.FirstOrDefault(o => o.Id == 1);
            Assert.NotNull(order);

            order.Total = 150;

            // Act
            var result = _controller.Edit(order.Id, order) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var updatedOrder = _context.Orders.Find(order.Id);
            Assert.NotNull(updatedOrder);
            Assert.Equal(150, updatedOrder.Total);
        }

        [Fact]
        public void Delete_OrderExists_ReturnsView()
        {
            // Act
            var result = _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Order>(result.Model);
            Assert.Equal(65, model.Total); // Validate the correct total
        }

        [Fact]
        public void Delete_OrderDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Delete(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void DeleteConfirmed_OrderExists_RedirectsToIndex()
        {
            // Act
            var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void DeleteConfirmed_OrderDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.DeleteConfirmed(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
