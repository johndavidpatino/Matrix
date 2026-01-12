using System;

namespace MatrixNext.Data.Services
{
    /// <summary>
    /// Generic API Response wrapper for all REST API endpoints
    /// Provides consistent response format with success/error handling
    /// </summary>
    /// <typeparam name="T">Data type returned on success</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response message (success or error description)
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Returned data (null if unsuccessful)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// HTTP Status Code
        /// </summary>
        public int StatusCode { get; set; } = 200;

        /// <summary>
        /// Total records (for paginated responses)
        /// </summary>
        public int? TotalRecords { get; set; }

        /// <summary>
        /// Error details (for debugging)
        /// </summary>
        public string? ErrorDetail { get; set; }

        /// <summary>
        /// Creates a successful response
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T? data, string message = "Success", int? totalRecords = null)
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

        /// <summary>
        /// Creates an error response
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400, string? errorDetail = null)
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

        /// <summary>
        /// Creates a not found response
        /// </summary>
        public static ApiResponse<T> NotFoundResponse(string message = "Resource not found")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = 404
            };
        }

        /// <summary>
        /// Creates an unauthorized response
        /// </summary>
        public static ApiResponse<T> UnauthorizedResponse(string message = "Unauthorized")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = 401
            };
        }
    }

    /// <summary>
    /// Non-generic API Response for operations that don't return data
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;
        public string? ErrorDetail { get; set; }

        public static ApiResponse SuccessResponse(string message = "Success")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                StatusCode = 200
            };
        }

        public static ApiResponse ErrorResponse(string message, int statusCode = 400, string? errorDetail = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                ErrorDetail = errorDetail
            };
        }
    }
}
