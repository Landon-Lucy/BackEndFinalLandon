using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("login", Schema = "Team2Part2")]
public partial class Login
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_id")]
    public int? CustomerId { get; set; }

    [Column("username")]
    [StringLength(100)]
    public string? Username { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Logins")]
    public virtual Customer? Customer { get; set; }
}
