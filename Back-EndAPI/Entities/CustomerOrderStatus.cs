using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_EndAPI.Entities;

[Table("customer_order_status", Schema = "Team2Part2")]
public partial class CustomerOrderStatus
{
    [Key]
    [Column("customer_order_id")]
    public int CustomerOrderId { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [ForeignKey("CustomerOrderId")]
    public virtual CustomerOrder? CustomerOrder { get; set; }
}
