using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[PrimaryKey("IdSupplier", "IdProduct", "Date")]
[Table("supplies")]
public partial class Supply
{
    [Key]
    [Column("id_supplier")]
    public int IdSupplier { get; set; }

    [Key]
    [Column("id_product")]
    public int IdProduct { get; set; }

    [Key]
    [Column("date", TypeName = "timestamp(0) without time zone")]
    public DateTime Date { get; set; }

    [Column("count")]
    public decimal Count { get; set; }

    [Column("cost")]
    public decimal Cost { get; set; }

    [Column("id_unit")]
    public int IdUnit { get; set; }

    [ForeignKey("IdProduct")]
    [InverseProperty("Supplies")]
    public virtual Product IdProductNavigation { get; set; } = null!;

    [ForeignKey("IdSupplier")]
    [InverseProperty("Supplies")]
    public virtual Supplier IdSupplierNavigation { get; set; } = null!;

    [ForeignKey("IdUnit")]
    [InverseProperty("Supplies")]
    public virtual MeasureUnit IdUnitNavigation { get; set; } = null!;
}
