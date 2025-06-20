namespace restaurant.server.Utils;

public class ServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public T? Data { get; private set; }

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T> { IsSuccess = true, Data = data };
    }

    public static ServiceResult<T> Fail(string? errorMessage)
    {
        return new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}