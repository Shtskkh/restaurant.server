using System.ComponentModel.DataAnnotations;

namespace restaurant.server.DTOs;

public class AddDishInOrderModel
{
    [Required] public required string Title { get; set; }
    public string? Comment { get; set; }
    [Required] public int Count { get; set; }
}