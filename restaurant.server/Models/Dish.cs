using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("dishes")]
public partial class Dish
{
    [Key]
    [Column("id_dish")]
    public int IdDish { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("cost")]
    public decimal Cost { get; set; }

    [Column("availability")]
    public bool Availability { get; set; }

    [Column("weight (volume)")]
    public decimal WeightVolume { get; set; }

    [Column("id_unit")]
    public int IdUnit { get; set; }

    [InverseProperty("IdDishNavigation")]
    public virtual ICollection<DishesInOrder> DishesInOrders { get; set; } = new List<DishesInOrder>();

    [ForeignKey("IdUnit")]
    [InverseProperty("Dishes")]
    public virtual MeasureUnit IdUnitNavigation { get; set; } = null!;

    [InverseProperty("IdDishNavigation")]
    public virtual ICollection<ProductsInDish> ProductsInDishes { get; set; } = new List<ProductsInDish>();
}
