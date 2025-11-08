using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkoutCatalogService.Shared.Response
{
    public class EndpointResponse<T>
    {
        private T Data { get; set; }
        private string Message { get; set; }
        private bool IsSuccess { get; set; }
        private int StatusCode { get; set; }

        public EndpointResponse(T data, int statusCode = 200, string message = "", bool isSuccess = true)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
            IsSuccess = isSuccess;
        }
        public EndpointResponse( int statusCode = 200, string message = "", bool isSuccess = true)
        {
            
            StatusCode = statusCode;
            Message = message;
            IsSuccess = isSuccess;
        }

       

        public static EndpointResponse<T> Success(T data, string message = "", int statusCode = 200)
        {
            return new EndpointResponse<T>(data, statusCode,message, true);
        }

        public static EndpointResponse<T> Fail(string message, int statusCode)
        {
            return new EndpointResponse<T>( statusCode, message, true);
        }


    }
}
