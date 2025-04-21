using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("tables")]
public partial class Table
{
    [Key]
    [Column("id_table")]
    public int IdTable { get; set; }

    [Column("number")]
    public int Number { get; set; }

    [InverseProperty("IdTableNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
