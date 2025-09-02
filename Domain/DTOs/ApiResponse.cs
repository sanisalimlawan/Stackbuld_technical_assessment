using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public int Code { get; set; }
        public ApiResponse(int StatusCode, string message, object? data, bool success)
        {
            Success = success;
            Message = message;
            Data = data;
            Code = StatusCode;
        }
    }
}
