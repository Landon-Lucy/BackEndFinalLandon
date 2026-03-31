using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("discrepancy", Schema = "Team2Part2")]
public partial class Discrepancy
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ordered_item_id")]
    public int? OrderedItemId { get; set; }

    [Column("received_item_id")]
    public int? ReceivedItemId { get; set; }

    [Column("description")]
    [StringLength(300)]
    public string? Description { get; set; }

    [ForeignKey("OrderedItemId")]
    [InverseProperty("Discrepancies")]
    public virtual OrderedItem? OrderedItem { get; set; }

    [ForeignKey("ReceivedItemId")]
    [InverseProperty("Discrepancies")]
    public virtual ReceivedItem? ReceivedItem { get; set; }
}
