using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("received_shipment", Schema = "Team2Part2")]
public partial class ReceivedShipment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("purchase_order_id")]
    public int? PurchaseOrderId { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("ReceivedShipments")]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    [InverseProperty("Shipment")]
    public virtual ICollection<ReceivedItem> ReceivedItems { get; set; } = new List<ReceivedItem>();
}
