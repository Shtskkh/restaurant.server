namespace restaurant.server.Utils;

public class RepositoryResult<T>
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public T? Data { get; set; }

    public static RepositoryResult<T> Success(T data)
    {
        return new RepositoryResult<T> { IsSuccess = true, Data = data };
    }

    public static RepositoryResult<T> Fail(string errorMessage)
    {
        return new RepositoryResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}