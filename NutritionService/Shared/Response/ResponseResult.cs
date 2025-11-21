namespace NutritionService.Shared.Response
{
    public class ResponseResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ResponseResult<T> SuccessResponse(T data, string? message = null)
        {
            return new ResponseResult<T> { Success = true, Data = data, Message = message };
        }
        public static ResponseResult<T> FailResponse(string message)
        {
            return new ResponseResult<T> { Success = false, Message = message };
        }
    }
}
