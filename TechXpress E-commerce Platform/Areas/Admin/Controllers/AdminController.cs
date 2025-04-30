using Microsoft.AspNetCore.Mvc;  
using TechXpress.Data.Entities;  
using Microsoft.AspNetCore.Authorization;    
using Microsoft.AspNetCore.Identity;
using TechXpress_E_commerce_Platform.Areas.Admin.ViewModels
    ;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace TechXpress.Controllers
{
    [Area("Admin")]     
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]   
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (isPasswordValid)
                    {  
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Role, "Admin")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);    
                      
                        await HttpContext.SignInAsync("AdminScheme", claimsPrincipal, new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe
                        });

                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                }
                ModelState.AddModelError("", "Invalid credentials or not an admin.");
            }
            return View(model);
        }

     
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminScheme");
            return RedirectToAction("Login");
        }
    }
}