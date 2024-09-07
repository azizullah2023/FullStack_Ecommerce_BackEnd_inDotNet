using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using WebApi_EcommerceReact.Data;
using WebApi_EcommerceReact.Models;

namespace WebApi_EcommerceReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private ApiResponse _response;
        public PaymentController(ApplicationDbContext db,IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {
            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).ThenInclude(u => u.MenuItem).FirstOrDefault(u=>u.UserId==userId);
            if (shoppingCart==null || shoppingCart.CartItems==null || shoppingCart.CartItems.Count==0) {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { "ShoppingCart or CartItem is null" };
                _response.IsSuccess = false;
                return BadRequest(_response);

            }
            #region Stripe Payment Intent
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            shoppingCart.CartTotal  = shoppingCart.CartItems.Sum(u=>u.Quantity*u.MenuItem.Price);
            var options = new PaymentIntentCreateOptions
            {
                Amount = (int)(shoppingCart.CartTotal * 100),  // convert amount to cents
                Currency = "usd",  // use the currency of your choice
                PaymentMethodTypes = new List<string> { "card" },  // specify allowed payment methods
            };
            var service = new PaymentIntentService();
            var response=service.Create(options);
            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;
            #endregion
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = shoppingCart;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
    }
