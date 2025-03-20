using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Repositories;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Interfaces;

namespace TechXpress_E_commerce.Controllers
{
    [Area("Admin")]
    public class OrderItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork ;

        public OrderItemController(IUnitOfWork unitOfWork)
        {
             unitOfWork= _unitOfWork;
        }

        // Get all order items
        public async Task<IActionResult> Index()
        {
            var orderItems = await  _unitOfWork.OrderItem.GetAllAsync();
            return View(orderItems);
        }

        // Get order item by ID
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var orderItem = await  _unitOfWork.OrderItem.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

       
    }
}
