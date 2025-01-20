using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce.Models;
using TechXpress_E_commerce.Models.AppDbContext;
using TechXpress_E_commerce.Repositories;

namespace TechXpress_E_commerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderController(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.GetAll()
                                          .AsQueryable()
                                          .Include(o => o.User)
                                          .Include(o => o.OrderItems)
                                          .Include(o => o.PaymentTransactions)
                                          .ToList();
            return View(orders);
        }
         
        public IActionResult Details(int id)
        { 
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        } 
        public IActionResult Create()
        {
            return View();
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.CreatedAt = DateTime.Now;
                order.UpdatedAt = DateTime.Now;
                _orderRepository.Add(order);
                _orderRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        } 
        public IActionResult Edit(int id)
        { 

            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                order.UpdatedAt = DateTime.Now;
                _orderRepository.Update(order);
                _orderRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        } 
        public IActionResult Delete(int id)
        {  
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        } 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order != null)
            {
                _orderRepository.Delete(order);
                _orderRepository.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }  
    }
}
