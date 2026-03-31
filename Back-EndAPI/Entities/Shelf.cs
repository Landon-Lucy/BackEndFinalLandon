using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("shelf", Schema = "Team2Part2")]
public partial class Shelf
{
    [Key]
    [Column("shelf_letter")]
    [MaxLength(1)]
    public char ShelfLetter { get; set; }

    [InverseProperty("ShelfLetterNavigation")]
    public virtual ICollection<AisleShelf> AisleShelves { get; set; } = new List<AisleShelf>();
}
