using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("item", Schema = "Team2Part2")]
public partial class Item
{
    [Key]
    [Column("sku_number")]
    public int SkuNumber { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("suggested_selling_price")]
    public int? SuggestedSellingPrice { get; set; }

    [Column("volume_per_unit")]
    public int? VolumePerUnit { get; set; }

    [InverseProperty("SkuNumberNavigation")]
    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

    [InverseProperty("SkuNumberNavigation")]
    public virtual ICollection<OrderedItem> OrderedItems { get; set; } = new List<OrderedItem>();

    [InverseProperty("SkuNumberNavigation")]
    public virtual ICollection<ReceivedItem> ReceivedItems { get; set; } = new List<ReceivedItem>();
}
