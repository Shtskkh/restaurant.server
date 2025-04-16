using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("positions")]
public partial class Position
{
    [Key]
    [Column("id_position")]
    public int IdPosition { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [InverseProperty("IdPositionNavigation")]
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
