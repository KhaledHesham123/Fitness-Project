using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkoutCatalogService.Shared.Response
{
    public class EndpointResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }

        public EndpointResponse(T data,  string message = "", bool isSuccess = true)
        {
            Data = data;
            Message = message;
            IsSuccess = isSuccess;
        }
        public EndpointResponse(  string message = "", bool isSuccess = true)
        {
            
            Message = message;
            IsSuccess = isSuccess;
        }

       

        public static EndpointResponse<T> Success(T data, string message = "", int statusCode = 200)
        {
            return new EndpointResponse<T>(data,message, true);
        }

        public static EndpointResponse<T> Fail(string message, int statusCode)
        {
            return new EndpointResponse<T>(  message, false);
        }


    }
}
