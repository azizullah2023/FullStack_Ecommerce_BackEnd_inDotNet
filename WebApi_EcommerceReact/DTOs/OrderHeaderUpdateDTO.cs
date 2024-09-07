using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApi_EcommerceReact.Models;

namespace WebApi_EcommerceReact.DTOs
{
    public class OrderHeaderUpdateDTO
    {
        public int Id { get; set; }
       
        public string? PickUpName { get; set; }
        
        public string? PickUpEmail { get; set; }
        public string ?PickUpPhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public string? Status { get; set; }

    }
}
