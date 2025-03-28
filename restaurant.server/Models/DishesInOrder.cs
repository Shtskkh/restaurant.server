using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[PrimaryKey("IdOrder", "IdDish")]
[Table("dishes_in_order")]
public partial class DishesInOrder
{
    [Key]
    [Column("id_order")]
    public int IdOrder { get; set; }

    [Key]
    [Column("id_dish")]
    public int IdDish { get; set; }

    [Column("count")]
    public int Count { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Column("id_status")]
    public int IdStatus { get; set; }

    [ForeignKey("IdDish")]
    [InverseProperty("DishesInOrders")]
    public virtual Dish IdDishNavigation { get; set; } = null!;

    [ForeignKey("IdOrder")]
    [InverseProperty("DishesInOrders")]
    public virtual Order IdOrderNavigation { get; set; } = null!;

    [ForeignKey("IdStatus")]
    [InverseProperty("DishesInOrders")]
    public virtual Status IdStatusNavigation { get; set; } = null!;
}
