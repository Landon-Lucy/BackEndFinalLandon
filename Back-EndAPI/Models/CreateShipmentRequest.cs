using System.ComponentModel.DataAnnotations;

namespace Back_EndAPI.Models;

public class CreateShipmentRequest
{
    [Required]
    public int PurchaseOrderId { get; set; }

    public DateTime? Date { get; set; }
}
