using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("customer_order", Schema = "Team2Part2")]
public partial class CustomerOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_id")]
    public int? CustomerId { get; set; }

    [Column("carrier_id")]
    public int? CarrierId { get; set; }

    [Column("date_time_ordered")]
    public DateOnly? DateTimeOrdered { get; set; }

    [Column("customer_shipping_fee")]
    [Precision(12, 2)]
    public decimal? CustomerShippingFee { get; set; }

    [InverseProperty("CustomerOrder")]
    public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();

    [ForeignKey("CarrierId")]
    [InverseProperty("CustomerOrders")]
    public virtual Carrier? Carrier { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CustomerOrders")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("CustomerOrder")]
    public virtual ICollection<SoldItem> SoldItems { get; set; } = new List<SoldItem>();
}
