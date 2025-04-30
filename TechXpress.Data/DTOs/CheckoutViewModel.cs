using System.ComponentModel.DataAnnotations;
using TechXpress.Data.Entities;

namespace TechXpress.Data.DTOs
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; } = new Order();  
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total => SubTotal + ShippingCost;
         
    }   

}