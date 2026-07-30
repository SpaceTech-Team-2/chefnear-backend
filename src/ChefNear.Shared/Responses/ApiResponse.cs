namespace ChefNear.Shared.Responses;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ApiResponse() { }

    public ApiResponse(int statusCode, bool isSuccess, string message, List<string>? errors = null)
    {
        StatusCode = statusCode;
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors ?? new List<string>();
    }

    public static ApiResponse SuccessResponse(string message = "Operation completed successfully.", int statusCode = 200)
    {
        return new ApiResponse(statusCode, true, message);
    }

    public static ApiResponse FailureResponse(string message, List<string>? errors = null, int statusCode = 400)
    {
        return new ApiResponse(statusCode, false, message, errors);
    }

    public static ApiResponse FailureResponse(string message, string error, int statusCode = 400)
    {
        return new ApiResponse(statusCode, false, message, new List<string> { error });
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse() { }

    public ApiResponse(int statusCode, bool isSuccess, string message, T? data, List<string>? errors = null)
        : base(statusCode, isSuccess, message, errors)
    {
        Data = data;
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully.", int statusCode = 200)
    {
        return new ApiResponse<T>(statusCode, true, message, data);
    }

    public static new ApiResponse<T> FailureResponse(string message, List<string>? errors = null, int statusCode = 400)
    {
        return new ApiResponse<T>(statusCode, false, message, default, errors);
    }

    public static new ApiResponse<T> FailureResponse(string message, string error, int statusCode = 400)
    {
        return new ApiResponse<T>(statusCode, false, message, default, new List<string> { error });
    }
}
