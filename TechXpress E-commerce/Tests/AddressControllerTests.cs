using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Controllers;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Repositories;
using Xunit;

namespace TechXpress_E_commerce.Tests
{
    public class AddressControllerTests
    {
        private readonly AddressController _controller;
        private readonly AppDbContext _context;
        private readonly Repository<Address> _addressRepository;
        private readonly Repository<User> _userRepository;

        public AddressControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _addressRepository = new Repository<Address>(_context);
            _userRepository = new Repository<User>(_context);
            SeedDatabase();

            _controller = new AddressController(_addressRepository, _userRepository);
        }

     private void SeedDatabase()
{
    // Add a User with all required properties
    var user = new User
    {
        Id = 1,
         FirstName = "User1",
        LastName = "Smith",
        Email = "user1@example.com",
        Password = "password123"
    };
    _userRepository.Add(user);
    _userRepository.SaveChanges(); 

    _addressRepository.Add(new Address
    {
        Id = 1,
        Street = "Street1",
        AddressType = "Home", 
        City = "City1",       
        State = "State1",      
        Country = "Country1",  
        PostalCode = "12345", 
        UserId = 1
    });
    _addressRepository.Add(new Address
    {
        Id = 2,
        Street = "Street2",
        AddressType = "Work",  
        City = "City2",       
        State = "State2",      
        Country = "Country2",  
        PostalCode = "67890",  
        UserId = 1
    });
    _addressRepository.SaveChanges();
}


        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfAddresses()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Address>>(result.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Details_AddressExists_ReturnsView()
        {
            // Act
            var result = _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Address>(result.Model);
            Assert.Equal("Street1", model.Street);
        }

        [Fact]
        public void Details_AddressDoesNotExist_ReturnsNotFound()
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
            _controller.ModelState.AddModelError("Street", "Street is required");
            var address = new Address();

            // Act
            var result = _controller.Create(address) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Address>(result.Model);
        }

        [Fact] 
        public void Create_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var address = new Address
            {
                Street = "Street3",
                AddressType = "Home",    
                City = "City3",          
                State = "State3",          
                Country = "Country3",    
                PostalCode = "54321",     
                UserId = 1               
            };

            // Act
            var result = _controller.Create(address) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }


        [Fact]
        public void Edit_AddressExists_ReturnsView()
        {
            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Address>(result.Model);
            Assert.Equal("Street1", model.Street);
        }

        [Fact]
        public void Edit_AddressDoesNotExist_ReturnsNotFound()
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
            var address = new Address { Id = 1, Street = "" };
            _controller.ModelState.AddModelError("Street", "Required");

            // Act
            var result = _controller.Edit(1, address) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Address>(result.Model);
        }

        [Fact]
        public void Edit_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var address = _context.Addresses.FirstOrDefault(a => a.Id == 1);
            Assert.NotNull(address);

            address.Street = "UpdatedStreet1";

            // Act
            var result = _controller.Edit(address.Id, address) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var updatedAddress = _context.Addresses.Find(address.Id);
            Assert.NotNull(updatedAddress);
            Assert.Equal("UpdatedStreet1", updatedAddress.Street);
        }

        [Fact]
        public void Delete_AddressExists_ReturnsView()
        {
            // Act
            var result = _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Address>(result.Model);
            Assert.Equal("Street1", model.Street);
        }

        [Fact]
        public void Delete_AddressDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.Delete(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void DeleteConfirmed_AddressExists_RedirectsToIndex()
        {
            // Act
            var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void DeleteConfirmed_AddressDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.DeleteConfirmed(100) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
