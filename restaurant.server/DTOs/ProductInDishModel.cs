namespace restaurant.server.DTOs;

public class ProductInDishModel
{
    public required int IdProduct { get; set; }
    public required string Title { get; set; }
    public required decimal Count { get; set; }
    public required string Unit { get; set; }
}