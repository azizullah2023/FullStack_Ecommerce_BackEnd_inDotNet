using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi_EcommerceReact.Models
{
    public class OrderHeader
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpEmail { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public double OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; }
    }
}
