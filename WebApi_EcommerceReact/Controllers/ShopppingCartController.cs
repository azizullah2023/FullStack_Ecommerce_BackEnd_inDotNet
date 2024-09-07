using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_EcommerceReact.Data;
using WebApi_EcommerceReact.Models;
using WebApi_EcommerceReact.Service;

namespace WebApi_EcommerceReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
       
        private ApiResponse _response;
        public ShopppingCartController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllShoppingCarts(string userId)
        {
            try {
                ShoppingCart shoppingcart;
                if (userId == null)
                {
                    shoppingcart = new();
                }
                else {
                   shoppingcart = _db.ShoppingCarts.Include(u => u.CartItems).ThenInclude(u => u.MenuItem).FirstOrDefault(u => u.UserId == userId);
                }
                if (shoppingcart ==null) {
                    ShoppingCart newcart = new() { UserId = userId };
                    _db.ShoppingCarts.Add(newcart);
                    await _db.SaveChangesAsync();
                }

                if (shoppingcart.CartItems!=null || shoppingcart.CartItems.Count>0) {

                    shoppingcart.CartTotal = shoppingcart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);
                }
                
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = shoppingcart;
                return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            
        }

            [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemsinShoppingCart(string userId,int menuItemId,int UpdateQuantityBy) {
            ShoppingCart cart = _db.ShoppingCarts.Include(u=>u.CartItems).FirstOrDefault(u=>u.UserId== userId);
            MenuItem newmenu = _db.MenuItems.FirstOrDefault(u => u.Id == menuItemId);
            if (newmenu == null) {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("MenuItem doesnt Exists.");
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if (cart == null && UpdateQuantityBy > 0)
            {
                ShoppingCart newcart = new() { UserId = userId };
                _db.ShoppingCarts.Add(newcart);
                await _db.SaveChangesAsync();
                CartItem cartitem = new()
                {
                    MenuItem = null,
                    MenuItemId = menuItemId,
                    ShoppingCartId = newcart.Id,

                };
                _db.CartItems.Add(cartitem);
                _db.SaveChanges();
            }
            else {
                CartItem cartIteminCart = cart.CartItems.FirstOrDefault(u => u.MenuItemId == menuItemId);
                if (cartIteminCart==null) {
                    CartItem newcartitem = new CartItem()
                    {
                        MenuItemId=menuItemId,
                        Quantity=UpdateQuantityBy,
                        ShoppingCartId=cart.Id,
                        MenuItem=null
                    };
                    _db.CartItems.Add(newcartitem);
                    _db.SaveChanges();
                }
                else {
                    int newQuantity = cartIteminCart.Quantity + UpdateQuantityBy;
                    if (UpdateQuantityBy==0 || newQuantity<=0) {
                        _db.CartItems.Remove(cartIteminCart);
                        if (cart.CartItems.Count()==1) 
                        {
                            _db.ShoppingCarts.Remove(cart);
                        }
                        _db.SaveChanges();
                    }
                    else {
                        cartIteminCart.Quantity = newQuantity;
                 
                    }
                   _db.CartItems.Update(cartIteminCart);
                    _db.SaveChanges();
                }
               
            
            }
            return _response;
        }
    }
}
