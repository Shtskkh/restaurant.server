namespace restaurant.server.DTOs;

public class AuthRequestModel
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}

public class AuthSuccessResponseModel
{
    public required string Token { get; set; }
}