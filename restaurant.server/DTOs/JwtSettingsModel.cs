namespace restaurant.server.DTOs;

public class JwtSettingsModel
{
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int AccessTokenExpirationMinutes { get; set; }
}