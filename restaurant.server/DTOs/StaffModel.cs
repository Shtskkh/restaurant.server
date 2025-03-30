namespace restaurant.server.DTOs;

public class StaffModel
{
    public int IdEmployee { get; set; }
    public int IdPosition { get; set; }
    public string Position { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string PhoneNumber { get; set; } = null!;
}