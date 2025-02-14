using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Repositories;

namespace TechXpress_E_commerce.Controllers
{
    public class AddressController : Controller
    {
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<ApplicationUser> _userRepository;

        public AddressController(IRepository<Address> addressRepository, IRepository<ApplicationUser> userRepository)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
        }

        // Get all addresses
        public IActionResult Index()
        {
            var addresses = _addressRepository.GetAllAsync();
            return View(addresses);
        }

        // Get address by ID
        [HttpGet]
        public IActionResult Details(int id)
        {
            var address = _addressRepository.GetByIdAsync(id);
            if (address == null)
                return NotFound();

            return View(address);
        }

        // Create (GET)
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = _userRepository.GetAllAsync();
            return View();
        }

        // Create  
        [HttpPost]
        public IActionResult Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _addressRepository.AddAsync(address);
                _addressRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // Edit  
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var address = _addressRepository.GetByIdAsync(id);
            if (address == null)
                return NotFound();  
            return View(address);
        }

        // Edit 
        [HttpPost]
        public IActionResult Edit(int id, Address address)
        {
            if (id != address.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _addressRepository.Update(address);
                _addressRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(address);
        }

        

    }
}
 