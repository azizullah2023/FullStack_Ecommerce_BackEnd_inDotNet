using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApi_EcommerceReact.Models;

namespace WebApi_EcommerceReact.DTOs
{
    public class OrderDetailCreateDTO
    {

        
        [Required]
        public int MenuItemId { get; set; }
       
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
