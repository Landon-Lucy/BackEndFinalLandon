using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("sold_item", Schema = "Team2Part2")]
public partial class SoldItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sku_number")]
    public int? SkuNumber { get; set; }

    [Column("customer_order_id")]
    public int? CustomerOrderId { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [ForeignKey("CustomerOrderId")]
    [InverseProperty("SoldItems")]
    public virtual CustomerOrder? CustomerOrder { get; set; }

    [ForeignKey("SkuNumber")]
    [InverseProperty("SoldItems")]
    public virtual Item? SkuNumberNavigation { get; set; }
}
