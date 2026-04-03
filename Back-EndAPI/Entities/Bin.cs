using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("bin", Schema = "Team2Part2")]
public partial class Bin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sku_number")]
    public int? SkuNumber { get; set; }

    [Column("qtystored")]
    public int? Qtystored { get; set; }

    [Column("aisle_bay_id")]
    public int? AisleBayId { get; set; }

    [Column("aisle_shelf_id")]
    public int? AisleShelfId { get; set; }

    [Column("height")]
    public int? Height { get; set; }

    [Column("volume")]
    public int? Volume { get; set; }

    [ForeignKey("AisleBayId")]
    [InverseProperty("Bins")]
    public virtual AisleBay? AisleBay { get; set; }

    [ForeignKey("AisleShelfId")]
    [InverseProperty("Bins")]
    public virtual AisleShelf? AisleShelf { get; set; }

    [ForeignKey("SkuNumber")]
    [InverseProperty("Bins")]
    public virtual Item? SkuNumberNavigation { get; set; }

    [InverseProperty("Storagelocation")]
    public virtual ICollection<TransferRecord> TransferRecords { get; set; } = new List<TransferRecord>();
}
