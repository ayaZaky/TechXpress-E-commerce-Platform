using Microsoft.AspNetCore.Mvc;
using TechXpress_E_commerce_Platform.View_Models;

namespace TechXpress_E_commerce_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {  
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalProducts = 150,
                ProductsChange = 12,
                TotalCategories = 12,
                TotalOrders = 1234,
                Revenue = 45678
            };
            return View(model);
        }
    }
}