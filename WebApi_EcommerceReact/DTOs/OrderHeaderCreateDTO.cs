using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApi_EcommerceReact.Models;

namespace WebApi_EcommerceReact.DTOs
{
    public class OrderHeaderCreateDTO
    {
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpEmail { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        public string ApplicationUserId { get; set; }

        public double OrderTotal { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<OrderDetailCreateDTO> orderDetailsDTO { get; set; }
    }
}
