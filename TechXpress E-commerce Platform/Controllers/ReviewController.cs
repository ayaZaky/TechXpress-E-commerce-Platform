using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Services.Interfaces;

namespace TechXpress_E_commerce_Platform.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public ReviewController( IUnitOfWork unitOfWork)
        {        
            _unitOfWork = unitOfWork;
        }    
        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, int rating, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = new Review
            {
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            await _unitOfWork.Review.AddAsync(review);
            await _unitOfWork.Review.SaveChangesAsync();  

            return Json(new { success = true });
        }

 




    }
}

