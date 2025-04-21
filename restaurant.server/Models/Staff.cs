using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant.server.Models;

[Table("staff")]
public partial class Staff
{
    [Key]
    [Column("id_employee")]
    public int IdEmployee { get; set; }

    [Column("id_position")]
    public int IdPosition { get; set; }

    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; } = null!;

    [Column("login")]
    public string Login { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } = null!;

    [ForeignKey("IdPosition")]
    [InverseProperty("Staff")]
    public virtual Position IdPositionNavigation { get; set; } = null!;

    [InverseProperty("IdEmployeeNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
