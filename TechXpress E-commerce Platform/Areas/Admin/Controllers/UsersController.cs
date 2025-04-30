using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Entities.Enums;
using TechXpress.Data.Repositories.Implementation;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress_E_commerce_Platform.Areas.Admin.ViewModels;

namespace TechXpress_E_commerce_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly  IUnitOfWork _unitOfWork;
   
        public UsersController(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;     
        }
        public async Task<IActionResult> Index()
        {   
            var users =await _unitOfWork.User.GetAllAsync();
            return View(users);
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]  // لحماية الـ CSRF
        public async Task<IActionResult> ToggleBlock(string userId, bool isBlocked)
        {
            try
            {
                Console.WriteLine("User ID received: " + userId); // طباعة الـ userId في السيرفر

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Invalid User ID" });
                }

                var user = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id.ToString() == userId);// تأكد من استخدام userId الصحيح
                if (user != null)
                {
                    user.IsBlocked = isBlocked;
                    await _unitOfWork.CompleteAsync();  // حفظ التغييرات في قاعدة البيانات
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "User not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

         

    }
}
