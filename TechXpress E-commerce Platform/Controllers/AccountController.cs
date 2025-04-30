using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;   
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Implementation;
using TechXpress.Data.Repositories.Interfaces; 
using TechXpress_E_commerce_Platform.View_Models;
using TechXpress_E_commerce_Platform.Models;
using TechXpress.Data.Entities.Enums;


namespace TechXpress_E_commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;   
        private readonly IEmailService  _emailService;
        private readonly IUnitOfWork _unitOfWork;





        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
             IEmailService emailService,
             IUnitOfWork unitOfwok
             )
            
        {
            _userManager = userManager;
            _signInManager = signInManager;    
            _emailService = emailService;
            _unitOfWork = unitOfWork;




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

                    Address = new Address
                    {
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        PostalCode = model.PostalCode ,
                        PhoneNumber = model.PhoneNumber,
                        Country = model.Country,
                        Type = AddressType.Mailing,

                    },
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

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", "Inner: " + ex.InnerException.Message);

                    if (ex.InnerException.InnerException != null)
                    {
                        ModelState.AddModelError("", "Inner Inner: " + ex.InnerException.InnerException.Message);
                    }
                }

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
                    // Check if user is blocked
                    if (user.IsBlocked)
                    {
                        ModelState.AddModelError(string.Empty, "This account is blocked. Please contact support.");
                        return View(model);
                    }

                    // Add claims
                    await _userManager.AddClaimAsync(user, new Claim("FirstName", user.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim("LastName", user.LastName));

                    // Try to login
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        model.Password,
                        model.RememberMe,
                        lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        TempData["Success"] = "You have successfully logged in!";
                        var roles = await _userManager.GetRolesAsync(user);
                        return RedirectToAction("Index", "Home");
                    }

                    if (result.IsLockedOut)
                    {
                        return View("Lockout");
                    }
                }

                ModelState.AddModelError(string.Empty, "Wrong credentials! Invalid Username or Password.");
            }

            return View(model);
        }

        // profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            //var user = await _userManager.GetUserAsync(User);
            var user = await _userManager.Users
              .Include(u => u.Orders)
                  .ThenInclude(o => o.OrderItems) 
              .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
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

                string body = $@"
                <div style='font-family:Segoe UI, sans-serif; padding: 30px; background-color: #f9f9f9; border: 1px solid #ddd; border-radius: 10px; max-width: 600px; margin: auto;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <img src='https://cdn-icons-png.flaticon.com/512/3064/3064197.png' alt='Reset Icon' width='60' style='margin-bottom: 10px;'/>
                        <h2 style='color: #333;'>Password Reset Request</h2>
                    </div>
                    <p style='font-size: 16px; color: #555;'>Hi <strong>{user.FirstName}</strong>,</p>
                    <p style='font-size: 15px; color: #555;'>We received a request to reset your password for your <strong>TechXpress</strong> account. Click the button below to proceed:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='padding: 12px 25px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                           Reset Password
                        </a>
                    </div>
                    <p style='font-size: 14px; color: #999;'>If you didn't request a password reset, you can safely ignore this email.</p>
                    <p style='font-size: 14px; color: #999;'>Regards,<br/>TechXpress Support Team</p>
                    </div>";


                await _emailService.SendAsync(model.Email, "Password Reset Request", body);

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
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
         
    }
}
