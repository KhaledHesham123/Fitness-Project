namespace UserProfileService.Shared.Response
{
    public class RequestResponse<T>
    {

        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }

        public RequestResponse(T data, int statusCode = 200, string message = "", bool isSuccess = true)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
            IsSuccess = isSuccess;
        }
        public RequestResponse(int statusCode = 200, string message = "", bool isSuccess = true)
        {

            StatusCode = statusCode;
            Message = message;
            IsSuccess = isSuccess;
        }



        public static RequestResponse<T> Success(T data, string message = "", int statusCode = 200)
        {
            return new RequestResponse<T>(data, statusCode, message, true);
        }

        public static RequestResponse<T> Fail(string message, int statusCode)
        {
            return new RequestResponse<T>(statusCode, message, false);
        }
    }
}
