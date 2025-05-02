namespace restaurant.server.DTOs;

public class SupplyModel
{
    public required string Product { get; set; }
    public required decimal Count { get; set; }
    public required DateTime Date { get; set; }
    public required string Unit { get; set; }
    public required string Supplier { get; set; }
}