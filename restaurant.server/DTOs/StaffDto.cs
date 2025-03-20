namespace restaurant.server.DTOs;

public class StaffDto
{
    public int IdEmployee { get; set; }
    public int IdPosition { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }
    public required string PhoneNumber { get; set; }
}