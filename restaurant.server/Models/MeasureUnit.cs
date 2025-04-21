using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("measure_units")]
public partial class MeasureUnit
{
    [Key]
    [Column("id_unit")]
    public int IdUnit { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [InverseProperty("IdUnitNavigation")]
    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();

    [InverseProperty("IdUnitNavigation")]
    public virtual ICollection<ProductsInDish> ProductsInDishes { get; set; } = new List<ProductsInDish>();

    [InverseProperty("IdUnitNavigation")]
    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
