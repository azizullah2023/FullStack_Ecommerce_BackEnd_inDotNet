using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApi_EcommerceReact.Data;
using WebApi_EcommerceReact.DTOs;
using WebApi_EcommerceReact.Models;
using WebApi_EcommerceReact.Utility;
using System.Linq;
using System.Text.Json;

namespace WebApi_EcommerceReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        private ApiResponse _response;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrder(string? userId,string?searchString,string?status,int pageNumber=1,int pageSize=5)
        {
            try
            {
                IEnumerable<OrderHeader> orderHeaders = _db.orderHeaders.Include(u => u.orderDetails).ThenInclude(u => u.MenuItems).OrderByDescending(u => u.Id);
                if (!string.IsNullOrEmpty(userId))
                {
                    orderHeaders = orderHeaders.Where(u => u.ApplicationUserId == userId).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    _response.Result = orderHeaders.Skip((pageNumber-1)* pageSize).Take(pageSize);
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    orderHeaders = _db.orderHeaders.Where(u => u.PickUpPhoneNumber.Contains(searchString) || u.PickUpName.Contains(searchString) || u.PickUpEmail.Contains(searchString));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    orderHeaders = _db.orderHeaders.Where(u => u.Status.ToLower() == status.ToLower());
                }
                Pagination pagination = new()
                {
                    PageSize = pageSize,
                    Currentpage = pageNumber,
                    TotalRecords = orderHeaders.Count()
                };
                Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(pagination));
                _response.Result = orderHeaders.Skip((pageNumber - 1) * pageSize).Take(pageSize);





            }
            catch (Exception)
            {

                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                _response.ErrorMessages = new List<string>() { "Order Header REcords doent Exist." };
                _response.IsSuccess = false;
            }
            return Ok(_response);


        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrder(int id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { $"The ID you is Null or 0" };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var orderHeaders = _db.orderHeaders.Include(u => u.orderDetails).ThenInclude(u => u.MenuItems).Where(u => u.Id == id);
                if (orderHeaders == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { $"The Specific Record with Id={id} is not Found" };
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    _response.Result = orderHeaders;
                    _response.StatusCode = System.Net.HttpStatusCode.OK;
                    return Ok(_response);

                }
            }
            catch (Exception ex)
            {

                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.IsSuccess = false;
            }
            return _response;


        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder(OrderHeaderCreateDTO dto)
        {
            try
            {
                OrderHeader order = new()
                {
                    PickUpName = dto.PickUpName,

                    PickUpEmail = dto.PickUpEmail,
                    PickUpPhoneNumber = dto.PickUpPhoneNumber,
                    ApplicationUserId = dto.ApplicationUserId,

                    OrderTotal = dto.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentIntentId = dto.StripePaymentIntentId,
                    Status = string.IsNullOrEmpty(dto.Status) ? SD.Status_Pending : dto.Status,
                    TotalItems = dto.TotalItems,

                };
                if (ModelState.IsValid) {
                    _db.orderHeaders.Add(order);
                    _db.SaveChanges();
                    foreach (var OrderDetailDto in dto.orderDetailsDTO)
                    {
                        OrderDetail orderdetail = new() {
                            OrderHeaderId = order.Id,
                            ItemName = OrderDetailDto.ItemName,
                            Price = OrderDetailDto.Price,
                            Quantity = OrderDetailDto.Quantity,
                            MenuItemId = OrderDetailDto.MenuItemId
                        };
                        _db.orderDetails.Add(orderdetail);
                    }
                    _db.SaveChanges();
                    _response.Result = order;
                    _response.StatusCode = System.Net.HttpStatusCode.Created;
                    order.orderDetails = null;
                    return Ok(_response);
                }
               
            }
            catch (Exception ex)
            {


                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.IsSuccess = false;
            }

            return _response;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateOrderHeader(int id,OrderHeaderUpdateDTO dto)
        {
            try
            {
                if ( dto==null || id!=dto.Id ) {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Id or data has not been passed in the fields." };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                OrderHeader orderFromDB = _db.orderHeaders.FirstOrDefault(u=>u.Id==id);
                if (orderFromDB == null) {
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { $"The Specific erocrd with Id={id} not found." };
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                if (!string.IsNullOrEmpty(dto.PickUpName))
                {
                    orderFromDB.PickUpName = dto.PickUpName;
                }
                if (!string.IsNullOrEmpty(dto.PickUpEmail)) {
                    orderFromDB.PickUpEmail = dto.PickUpEmail;
                }
                if (!string.IsNullOrEmpty(dto.PickUpPhoneNumber))
                {
                    orderFromDB.PickUpPhoneNumber = dto.PickUpPhoneNumber;
                }
                if (!string.IsNullOrEmpty(dto.Status))
                {
                    orderFromDB.Status = dto.Status;
                }
                if (!string.IsNullOrEmpty(dto.StripePaymentIntentId))
                {
                    orderFromDB.StripePaymentIntentId = dto.StripePaymentIntentId;
                }
                _db.SaveChanges();
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                return (_response);

            }
          
            catch (Exception ex)
            {


                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.IsSuccess = false;
            }
            return _response;


        }
        }
}
