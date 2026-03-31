using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("received_item", Schema = "Team2Part2")]
public partial class ReceivedItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sku_number")]
    public int? SkuNumber { get; set; }

    [Column("shipment_id")]
    public int? ShipmentId { get; set; }

    [Column("qty")]
    public int? Qty { get; set; }

    [InverseProperty("ReceivedItem")]
    public virtual ICollection<Discrepancy> Discrepancies { get; set; } = new List<Discrepancy>();

    [InverseProperty("ReceivedItem")]
    public virtual ICollection<ReceivedHistory> ReceivedHistories { get; set; } = new List<ReceivedHistory>();

    [ForeignKey("ShipmentId")]
    [InverseProperty("ReceivedItems")]
    public virtual ReceivedShipment? Shipment { get; set; }

    [ForeignKey("SkuNumber")]
    [InverseProperty("ReceivedItems")]
    public virtual Item? SkuNumberNavigation { get; set; }

    [InverseProperty("Receiveditem")]
    public virtual ICollection<TransferRecord> TransferRecords { get; set; } = new List<TransferRecord>();
}
