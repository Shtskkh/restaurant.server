namespace restaurant.server.DTOs;

public class StaffModel
{
    public required int IdEmployee { get; set; }
    public required string Position { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string PhoneNumber { get; set; }
}