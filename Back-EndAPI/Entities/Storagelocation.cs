using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("storagelocation", Schema = "Team2Part2")]
public partial class Storagelocation
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

    [ForeignKey("AisleBayId")]
    [InverseProperty("Storagelocations")]
    public virtual AisleBay? AisleBay { get; set; }

    [ForeignKey("AisleShelfId")]
    [InverseProperty("Storagelocations")]
    public virtual AisleShelf? AisleShelf { get; set; }

    [InverseProperty("Storagelocation")]
    public virtual ICollection<TransferRecord> TransferRecords { get; set; } = new List<TransferRecord>();
}
