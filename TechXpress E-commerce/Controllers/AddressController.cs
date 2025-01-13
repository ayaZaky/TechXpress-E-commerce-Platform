using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace TechXpress_E_commerce.Controllers
{
    public class AddressController : Controller
    {
        private readonly AppDbContext _context;

        public AddressController(AppDbContext context)
        {
            _context = context;
        }

        // Get all addresses
        public IActionResult Index()
        {
            var addresses = _context.Addresses
                                    .Include(a => a.User)
                                    .ToList();
            return View(addresses);
        }

        // Get address by ID
        [HttpGet]
        public IActionResult Details(int id)
        {
            var address = _context.Addresses
                                  .Include(a => a.User)
                                  .FirstOrDefault(a => a.Id == id);

            if (address == null)
                return NotFound();

            return View(address);
        }

        // Create (GET)
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            return View();
        }

        // Create (POST)
        [HttpPost]
        public IActionResult Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Addresses.Add(address);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }  
            return View(address);
        }

        // Edit (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null)
                return NotFound();
 
            return View(address);
        }

        // Edit (POST)
        [HttpPost]
        public IActionResult Edit(int id, Address address)
        {
            if (id != address.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(address);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
             
            return View(address);
        }

        // Delete (GET)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var address = _context.Addresses
                                  .Include(a => a.User)
                                  .FirstOrDefault(a => a.Id == id);
            if (address == null)
                return NotFound();

            return View(address);
        }

        // Delete (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null)
                return NotFound();

            _context.Addresses.Remove(address);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
 