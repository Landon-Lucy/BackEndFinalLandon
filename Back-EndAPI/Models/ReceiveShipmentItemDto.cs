using System.ComponentModel.DataAnnotations;

namespace Back_EndAPI.Models;

public class ReceiveShipmentItemDto
{
    [Required]
    public int SkuNumber { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Qty { get; set; }
}
