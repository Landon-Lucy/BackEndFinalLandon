using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("purchase_order", Schema = "Team2Part2")]
public partial class PurchaseOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("date_ordered")]
    public DateOnly DateOrdered { get; set; }

    [Column("vendorid")]
    public int? Vendorid { get; set; }

    [Column("expected_total_cost")]
    public int? ExpectedTotalCost { get; set; }

    [InverseProperty("Purchase")]
    public virtual ICollection<OrderedItem> OrderedItems { get; set; } = new List<OrderedItem>();

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<ReceivedShipment> ReceivedShipments { get; set; } = new List<ReceivedShipment>();

    [ForeignKey("Vendorid")]
    [InverseProperty("PurchaseOrders")]
    public virtual Vendor? Vendor { get; set; }
}
