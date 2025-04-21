using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("products")]
public partial class Product
{
    [Key]
    [Column("id_product")]
    public int IdProduct { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [InverseProperty("IdProductNavigation")]
    public virtual ICollection<ProductsInDish> ProductsInDishes { get; set; } = new List<ProductsInDish>();

    [InverseProperty("IdProductNavigation")]
    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
