using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Implementation;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress_E_commerce_Platform.View_Models;

namespace TechXpress_E_commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;   
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
      

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;    
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);
               
                if (result.Succeeded)
                {
                    // Add Role Here
                    await _userManager.AddToRoleAsync(user, "Customer");
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _userManager.AddClaimAsync(user, new Claim("FirstName", user.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim("LastName",  user.LastName));
                }

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        model.Password,
                        model.RememberMe,
                        lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        TempData["Successs"] = "You have successfully logged in!";
                        var roles = await _userManager.GetRolesAsync(user); 
                        // Redirect Based on Role 
                            return RedirectToAction("Index", "Home");    
                    }

                    if (result.IsLockedOut)
                    {
                        return View("Lockout");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }
        // profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(user);

        }
        //Upate Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  
            }  
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            } 
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;   
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }  
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);   
        }
        //Change Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["PasswordError"] = "Please correct the errors and try again.";
                return RedirectToAction("Profile");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isOldPasswordValid)
            {
                TempData["PasswordError"] = "Current password is incorrect.";
                return RedirectToAction("Profile");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            TempData["PasswordError"] = "Failed to change password.";
            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "No user found with that email address.");
                    return View(model);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
     
                var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, protocol: Request.Scheme);
         

                // Send the reset link via email
                await _emailService.SendAsync(model.Email, "Password Reset Request",
                    $"Please click on the link to reset your password: {resetLink}");

                return View("ForgotPasswordConfirmation");  // Or return any confirmation view
            }

            return View(model);  // If validation fails
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]  
        //public async Task<IActionResult> Edit(ApplicationUser model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        if (user == null) return NotFound("User not found");

        //        user.FirstName = model.FirstName;
        //        user.LastName = model.LastName;
        //        user.Email = model.Email;
        //        user.PhoneNumber = model.PhoneNumber;

        //        var result = await _userManager.UpdateAsync(user);

        //        if (result.Succeeded)
        //        {
        //            return Json(new { success = true });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Error updating profile." });
        //        }
        //    }
        //    return Json(new { success = false, message = "Invalid data." });
        //} 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
         
    }
}
