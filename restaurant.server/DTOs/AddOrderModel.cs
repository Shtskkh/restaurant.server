using System.ComponentModel.DataAnnotations;

namespace restaurant.server.DTOs;

public class AddOrderModel
{
    [Required] public required string EmployeeLogin { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public int Table { get; set; }
    [Required] public required List<AddDishInOrderModel> Dishes { get; set; }
}