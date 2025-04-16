namespace restaurant.server.DTOs;

public class DishInOrderModel
{
    public required int IdDish { get; set; }
    public required string Title { get; set; }
    public required int Count { get; set; }
    public string? Comment { get; set; }
    public required string Status { get; set; }
    public required decimal TotalCost { get; set; }
}