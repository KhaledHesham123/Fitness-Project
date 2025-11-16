namespace IdentityService.Features.Shared
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static Result<T> Response(bool success)
        {
            return new Result<T> { Success = success };
        }
        public static Result<T> SuccessResponse(T data, string? message = null)
        {
            return new Result<T> { Success = true, Data = data, Message = message };
        }
        public static Result<T> FailResponse(string message)
        {
            return new Result<T> { Success = false, Message = message };
        }
    }
}
