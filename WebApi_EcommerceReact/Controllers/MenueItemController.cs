using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebApi_EcommerceReact.Data;
using WebApi_EcommerceReact.DTOs;
using WebApi_EcommerceReact.Models;
using WebApi_EcommerceReact.Service;
using WebApi_EcommerceReact.Utility;

namespace WebApi_EcommerceReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenueItemController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IBlobService blobService;
        private  ApiResponse _response;
        public MenueItemController(ApplicationDbContext db,IBlobService blobService)
        {
            _db = db;
            this.blobService = blobService;
            _response = new ApiResponse();
        }
        [HttpGet]
        public IActionResult GetAllMenueItems() {
            _response.Result = _db.MenuItems;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}",Name ="GetMenuItems")]
        public IActionResult GetAllMenueItems(int id)
        {
            if (id==null || id==0) {
                return BadRequest("Id not found or Id is null");
            }
            _response.Result = _db.MenuItems.FirstOrDefault(x=>x.Id==id);
            if (_response.Result==null)
            {
                
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm]MenuItemCreateDto menuItemCreateDto) 
        {
            try
            {
                if (ModelState.IsValid) {
                    if (menuItemCreateDto.File==null || menuItemCreateDto.File.Length==0)
                        {
                        return BadRequest("File is required");
                    }
                    string filename = $"{Guid.NewGuid()}{Path.GetExtension(menuItemCreateDto.File.FileName)}";
                    MenuItem menuItemToCreate = new() {
                        Name = menuItemCreateDto.Name,
                        Description = menuItemCreateDto.Description,
                        SpecialTag = menuItemCreateDto.SpecialTag,
                        Category = menuItemCreateDto.Category,
                        Price = menuItemCreateDto.Price,
                        Image = await blobService.UploadBlob(filename, SD.imageContainer, menuItemCreateDto.File)
                        
                    };
                    _db.MenuItems.Add(menuItemToCreate);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.Result = menuItemToCreate;
                    return CreatedAtRoute("GetMenuItems", new { id = menuItemToCreate.Id }, _response);

                }
                else {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "ModelState is not Valid" };
                    return _response;
                }
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string>() { ex.ToString()};
                return _response;
            }
        
        
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDto? menuItemUpdateDto)
        {
            if (menuItemUpdateDto == null || !ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            try
            {
                var rowFromDB = await _db.MenuItems.FindAsync(id);
                if (rowFromDB == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Menu item not found." }
                    });
                }

                // Update fields if provided
                if (!string.IsNullOrEmpty(menuItemUpdateDto.Name))
                {
                    rowFromDB.Name = menuItemUpdateDto.Name;
                }
                if (!string.IsNullOrEmpty(menuItemUpdateDto.Description))
                {
                    rowFromDB.Description = menuItemUpdateDto.Description;
                }
                if (!string.IsNullOrEmpty(menuItemUpdateDto.SpecialTag))
                {
                    rowFromDB.SpecialTag = menuItemUpdateDto.SpecialTag;
                }
                if (!string.IsNullOrEmpty(menuItemUpdateDto.Category))
                {
                    rowFromDB.Category = menuItemUpdateDto.Category;
                }
                if (menuItemUpdateDto.Price.HasValue)
                {
                    rowFromDB.Price = menuItemUpdateDto.Price.Value;
                }

                // Handle file upload if provided
                if (menuItemUpdateDto.File != null && menuItemUpdateDto.File.Length > 0)
                {
                    string filename = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDto.File.FileName)}";
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(rowFromDB.Image))
                    {
                        await blobService.DeleteBlob(rowFromDB.Image.Split('/').Last(), SD.imageContainer);
                    }
                    // Upload the new image
                    rowFromDB.Image = await blobService.UploadBlob(filename, SD.imageContainer, menuItemUpdateDto.File);
                }

                // Update the database record
                _db.MenuItems.Update(rowFromDB);
                await _db.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Result = rowFromDB,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                });
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
        {
                    if (id==0 ||id ==null)
                    {
                        return BadRequest("File is required");
                    }
                   
                    var RowFromDB = await _db.MenuItems.FindAsync(id);



                    var deletefilename=RowFromDB.Image.Split('/').Last();
                   await blobService.DeleteBlob(deletefilename, SD.imageContainer);


                    _db.MenuItems.Remove(RowFromDB);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.Result = "the roww deleted successfully";
                    return Ok(_response);



        }
    }
}
