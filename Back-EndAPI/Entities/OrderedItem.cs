using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("ordered_item", Schema = "Team2Part2")]
public partial class OrderedItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sku_number")]
    public int? SkuNumber { get; set; }

    [Column("purchase_id")]
    public int? PurchaseId { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("cost_per_unit")]
    [Precision(12, 2)]
    public decimal CostPerUnit { get; set; }

    [InverseProperty("OrderedItem")]
    public virtual ICollection<Discrepancy> Discrepancies { get; set; } = new List<Discrepancy>();

    [ForeignKey("PurchaseId")]
    [InverseProperty("OrderedItems")]
    public virtual PurchaseOrder? Purchase { get; set; }

    [ForeignKey("SkuNumber")]
    [InverseProperty("OrderedItems")]
    public virtual Item? SkuNumberNavigation { get; set; }
}
