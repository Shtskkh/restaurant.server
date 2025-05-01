namespace restaurant.server.DTOs;

public class AuthRequestModel
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}