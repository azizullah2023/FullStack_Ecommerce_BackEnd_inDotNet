using System.Net;

namespace WebApi_EcommerceReact.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; } 
        public object Result { get; set; }
    }
}
