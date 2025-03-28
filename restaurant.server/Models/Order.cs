using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("id_order")]
    public int IdOrder { get; set; }

    [Column("date", TypeName = "timestamp(0) without time zone")]
    public DateTime Date { get; set; }

    [Column("id_table")]
    public int IdTable { get; set; }

    [Column("id_status")]
    public int IdStatus { get; set; }

    [Column("id_employee")]
    public int IdEmployee { get; set; }

    [InverseProperty("IdOrderNavigation")]
    public virtual ICollection<DishesInOrder> DishesInOrders { get; set; } = new List<DishesInOrder>();

    [ForeignKey("IdEmployee")]
    [InverseProperty("Orders")]
    public virtual Staff IdEmployeeNavigation { get; set; } = null!;

    [ForeignKey("IdStatus")]
    [InverseProperty("Orders")]
    public virtual Status IdStatusNavigation { get; set; } = null!;

    [ForeignKey("IdTable")]
    [InverseProperty("Orders")]
    public virtual Table IdTableNavigation { get; set; } = null!;
}
