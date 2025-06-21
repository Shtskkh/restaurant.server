namespace restaurant.server.DTOs;

public class AddStaffModel
{
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string Position { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
}