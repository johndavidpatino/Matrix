using System;

namespace MatrixNext.Data.Services
{
    /// <summary>
    /// Generic API Response wrapper for all REST API endpoints
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int StatusCode { get; set; } = 200;
        public int? TotalRecords { get; set; }
        public string? ErrorDetail { get; set; }

        public static ApiResponse<T> Ok(T? data, string message = "Success", int? totalRecords = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200,
                TotalRecords = totalRecords
            };
        }

        public static ApiResponse<T> Error(string message, int statusCode = 400, string? errorDetail = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = statusCode,
                ErrorDetail = errorDetail
            };
        }
    }
}
