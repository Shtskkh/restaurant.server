using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("suppliers")]
public partial class Supplier
{
    [Key]
    [Column("id_supplier")]
    public int IdSupplier { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("inn")]
    public string Inn { get; set; } = null!;

    [Column("kpp")]
    public string Kpp { get; set; } = null!;

    [InverseProperty("IdSupplierNavigation")]
    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
