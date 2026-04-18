using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("box", Schema = "Team2Part2")]
public partial class Box
{
    [Key]
    [Column("tracking")]
    public int Tracking { get; set; }

    [Column("volume")]
    [Precision(10, 2)]
    public decimal? Volume { get; set; }

    [Column("carrier_shipping_fee")]
    [Precision(10, 2)]
    public decimal? CarrierShippingFee { get; set; }

    [Column("customer_order_id")]
    public int? CustomerOrderId { get; set; }

    [Column("date_shipped")]
    public DateOnly? DateShipped { get; set; }

    [ForeignKey("CustomerOrderId")]
    [InverseProperty("Boxes")]
    public virtual CustomerOrder? CustomerOrder { get; set; }

    [InverseProperty("BoxTrackingNavigation")]
    public virtual ICollection<ShippedItem> ShippedItems { get; set; } = new List<ShippedItem>();
}
