using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("aisle_shelf", Schema = "Team2Part2")]
public partial class AisleShelf
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shelf_letter")]
    [MaxLength(1)]
    public char? ShelfLetter { get; set; }

    [Column("aisle_number")]
    public int? AisleNumber { get; set; }

    [Column("shelf_height")]
    public int? ShelfHeight { get; set; }

    [ForeignKey("AisleNumber")]
    [InverseProperty("AisleShelves")]
    public virtual Aisle? AisleNumberNavigation { get; set; }

    [InverseProperty("AisleShelf")]
    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

    [ForeignKey("ShelfLetter")]
    [InverseProperty("AisleShelves")]
    public virtual Shelf? ShelfLetterNavigation { get; set; }
}
