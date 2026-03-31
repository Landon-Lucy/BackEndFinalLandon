using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("bay", Schema = "Team2Part2")]
public partial class Bay
{
    [Key]
    [Column("bay_number")]
    public int BayNumber { get; set; }

    [InverseProperty("BayNumberNavigation")]
    public virtual ICollection<AisleBay> AisleBays { get; set; } = new List<AisleBay>();
}
