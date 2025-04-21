using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("statuses")]
public partial class Status
{
    [Key]
    [Column("id_status")]
    public int IdStatus { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [InverseProperty("IdStatusNavigation")]
    public virtual ICollection<DishesInOrder> DishesInOrders { get; set; } = new List<DishesInOrder>();

    [InverseProperty("IdStatusNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
