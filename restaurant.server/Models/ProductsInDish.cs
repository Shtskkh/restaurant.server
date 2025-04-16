using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[PrimaryKey("IdDish", "IdProduct")]
[Table("products_in_dish")]
public partial class ProductsInDish
{
    [Key]
    [Column("id_dish")]
    public int IdDish { get; set; }

    [Key]
    [Column("id_product")]
    public int IdProduct { get; set; }

    [Column("count")]
    public decimal Count { get; set; }

    [Column("id_unit")]
    public int IdUnit { get; set; }

    [ForeignKey("IdDish")]
    [InverseProperty("ProductsInDishes")]
    public virtual Dish IdDishNavigation { get; set; } = null!;

    [ForeignKey("IdProduct")]
    [InverseProperty("ProductsInDishes")]
    public virtual Product IdProductNavigation { get; set; } = null!;

    [ForeignKey("IdUnit")]
    [InverseProperty("ProductsInDishes")]
    public virtual MeasureUnit IdUnitNavigation { get; set; } = null!;
}
