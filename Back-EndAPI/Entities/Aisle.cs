using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("aisle", Schema = "Team2Part2")]
public partial class Aisle
{
    [Key]
    [Column("aisle_number")]
    public int AisleNumber { get; set; }

    [InverseProperty("AisleNumberNavigation")]
    public virtual ICollection<AisleBay> AisleBays { get; set; } = new List<AisleBay>();

    [InverseProperty("AisleNumberNavigation")]
    public virtual ICollection<AisleShelf> AisleShelves { get; set; } = new List<AisleShelf>();
}
