using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("aisle_bay", Schema = "Team2Part2")]
public partial class AisleBay
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("bay_number")]
    public int? BayNumber { get; set; }

    [Column("aisle_number")]
    public int? AisleNumber { get; set; }

    [ForeignKey("AisleNumber")]
    [InverseProperty("AisleBays")]
    public virtual Aisle? AisleNumberNavigation { get; set; }

    [ForeignKey("BayNumber")]
    [InverseProperty("AisleBays")]
    public virtual Bay? BayNumberNavigation { get; set; }

    [InverseProperty("AisleBay")]
    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();
}
