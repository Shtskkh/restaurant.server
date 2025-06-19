namespace restaurant.server.Utils;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T> { IsSuccess = true, Data = data };
    }

    public static ServiceResult<T> Fail(string? errorMessage)
    {
        return new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}