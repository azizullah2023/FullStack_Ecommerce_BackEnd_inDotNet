

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_EcommerceReact.Data;
using WebApi_EcommerceReact.DTOs;
using WebApi_EcommerceReact.Models;
using WebApi_EcommerceReact.Service;
using WebApi_EcommerceReact.Utility;

namespace WebApi_EcommerceReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _jwtKey;
        private readonly ApiResponse _response;

        public AuthController(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtKey = configuration.GetValue<string>("jwt:secretkey");
            _response = new ApiResponse();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse>> LoginUser([FromBody] LoginRequestDTO dto)
        {
            var userfromDB = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == dto.UserName.ToLower());
            if (userfromDB == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName or Password is Invalid");
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            bool isValid = await _userManager.CheckPasswordAsync(userfromDB, dto.Password);
            if (!isValid)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName or Password is Invalid");
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            // JWT Token Creation
            var roles = await _userManager.GetRolesAsync(userfromDB);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_jwtKey);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", userfromDB.Id.ToString()),
                    new Claim("fullName", userfromDB.Name),
                    new Claim(ClaimTypes.Email, userfromDB.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            LoginResponseDTO loginUser = new()
            {
                Email = userfromDB.Email,
                Token = tokenString
            };

            if (loginUser.Email == null || string.IsNullOrEmpty(loginUser.Token))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName or Password is Invalid");
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = loginUser;
            return Ok(_response);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse>> RegisterUser([FromBody] RegisterDTO dto)
        {
            var userfromDB = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == dto.UserName.ToLower());
            if (userfromDB != null)
            {
                _response.IsSuccess = false;
                if (_response.ErrorMessages == null)
                {
                    _response.ErrorMessages = new List<string>();
                }
                _response.ErrorMessages.Add("UserName already exists");
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            ApplicationUser newuser = new()
            {
                Name = dto.Name,
                UserName = dto.UserName,
                NormalizedEmail = dto.UserName.ToUpper(),
                Email = dto.UserName
            };

            var result = await _userManager.CreateAsync(newuser, dto.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                }

                if (dto.Role.Trim().ToLower() == SD.Role_Admin)
                {
                    await _userManager.AddToRoleAsync(newuser, SD.Role_Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newuser, SD.Role_Customer);
                }

                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = newuser;
                return Ok(_response);
            }

            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Error occurred while Registration!");
            return BadRequest(_response);
        }
    }
}

