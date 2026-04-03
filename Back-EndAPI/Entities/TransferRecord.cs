using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Entities;

[Table("transfer_record", Schema = "Team2Part2")]
public partial class TransferRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("storagelocationid")]
    public int? Storagelocationid { get; set; }

    [Column("withdrawl")]
    public bool? Withdrawl { get; set; }

    [Column("deposit")]
    public bool? Deposit { get; set; }

    [Column("qty")]
    public int? Qty { get; set; }

    [Column("receiveditemid")]
    public int? Receiveditemid { get; set; }

    [Column("datetime", TypeName = "timestamp without time zone")]
    public DateTime? Datetime { get; set; }

    [ForeignKey("Receiveditemid")]
    [InverseProperty("TransferRecords")]
    public virtual ReceivedItem? Receiveditem { get; set; }

    [ForeignKey("Storagelocationid")]
    [InverseProperty("TransferRecords")]
    public virtual Bin? Storagelocation { get; set; }
}
