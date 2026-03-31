using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("received_history", Schema = "Team2Part2")]
public partial class ReceivedHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("received_item_id")]
    public int? ReceivedItemId { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("comment")]
    [StringLength(1000)]
    public string? Comment { get; set; }

    [ForeignKey("ReceivedItemId")]
    [InverseProperty("ReceivedHistories")]
    public virtual ReceivedItem? ReceivedItem { get; set; }
}
