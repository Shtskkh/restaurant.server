namespace restaurant.server.DTOs;

public class DishModel
{
    public required int IdDish { get; set; }
    public required string Title { get; set; }
    public required decimal Cost { get; set; }
    public required bool Availability { get; set; }
    public required decimal WeightVolume { get; set; }
    public required string Unit { get; set; }
}