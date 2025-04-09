namespace restaurant.server.DTOs;

public class OrderModel
{
    public int IdOrder { get; set; }
    public DateTime Date { get; set; }
    public int TableNumber { get; set; }
    public string Status { get; set; }
    public string? Employee { get; set; }
}