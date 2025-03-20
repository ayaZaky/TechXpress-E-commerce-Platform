using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress_E_commerce.Repositories;
using TechXpress.Data.Entities;
using TechXpress.Data.Entities.Enums;

namespace TechXpress_E_commerce.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _unitOfWork.Order.GetAllOrdersWithUserAndItemsAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
              

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await   _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = status;
            _unitOfWork.Order.Update(order);
            await _unitOfWork.Order.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }



        public async Task<IActionResult> Delete(int id)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _unitOfWork.Order.Remove(order);  
            _unitOfWork.Order.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
 
   
}
