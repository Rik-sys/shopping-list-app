using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string? ErrorCode { get; set; }

        public ApiResponse(T? data, string message)
        {
            Success = true;
            Message = message;
            Data = data;
            Timestamp = DateTime.UtcNow;
        }

        public ApiResponse(string errorMessage, string? errorCode = null)
        {
            Success = false;
            Message = errorMessage;
            Data = default;
            Timestamp = DateTime.UtcNow;
            ErrorCode = errorCode;
        }
    }
}
