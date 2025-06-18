namespace Nasteafy.Application.Common.Models
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public required string ErrorMessage { get; set; }
    }
} 
