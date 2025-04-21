namespace restaurant.server.Controllers;

public class OrderModel
{
    public required int IdOrder { get; set; }
    public required DateTime Date { get; set; }
    public required int TableNumber { get; set; }
    public required string Status { get; set; }
    public required string Employee { get; set; }
    public List<DishInOrderModel>? DishesInOrder { get; set; }
}