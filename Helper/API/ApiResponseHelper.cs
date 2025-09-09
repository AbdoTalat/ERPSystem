using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Helper.Constants;

namespace Helper.API
{
    public class ApiResponseHelper<T> where T : class
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponseHelper(bool _success, string _message, int _statusCode, T _Data)
        {
            Success = _success;
            Status = GetStatusNameForCode(_statusCode);
            StatusCode = _statusCode;
            Message = _message;
            Data = _Data;
        }
        public ApiResponseHelper(bool _success, string _message, int _statusCode)
        {
            Success = _success;
            Status = GetStatusNameForCode(_statusCode);
            StatusCode = _statusCode;
            Message = _message;
            Data = null;
        }


        public static ApiResponseHelper<T> ResponseSuccess(int statusCode = StatusCodes.OK, string? message = null, T? data = null)
        {
            return new ApiResponseHelper<T>(true, message ?? GetDefaultMessageForStatusCode(statusCode), statusCode, data);
        }

        public static ApiResponseHelper<T> ResponseFailure(int statusCode, string? message = null)
        {
            return new ApiResponseHelper<T>(false, message ?? GetDefaultMessageForStatusCode(statusCode), statusCode);
        }


        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.BAD_REQUEST => "Bad Request",
                StatusCodes.UNAUTHORIZED => "Unauthorized",
                StatusCodes.FORBIDDEN => "Forbidden",
                StatusCodes.NOT_FOUND => "Resource Not Found",
                StatusCodes.INTERNAL_SERVER_ERROR => "Internal Server Error",
                StatusCodes.NO_CONTENT => "No Content",
                StatusCodes.CREATED => "Resource Created Successfully",
                StatusCodes.OK => "Request Successful",
                StatusCodes.CONFLICT => "Conflict with existing data or constraints",
                _ => "An unexpected error occurred"
            };
        }

        private static string GetStatusNameForCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.CREATED => "Created",
                StatusCodes.BAD_REQUEST => "BadRequest",
                StatusCodes.UNAUTHORIZED => "Unauthorized",
                StatusCodes.FORBIDDEN => "Forbidden",
                StatusCodes.NOT_FOUND => "NotFound",
                StatusCodes.INTERNAL_SERVER_ERROR => "InternalServerError",
                StatusCodes.NO_CONTENT => "NoContent",
                StatusCodes.OK => "OK",
                StatusCodes.CONFLICT => "Conflict",
                _ => "Unknown"
            };
        }
    }
}
