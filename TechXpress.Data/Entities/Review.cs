using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Data.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // 1 to 5 stars

        [Required]
        [StringLength(500)]
        public string Comment { get; set; } // Review text

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Key - Assuming a Review is linked to a Product
        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
