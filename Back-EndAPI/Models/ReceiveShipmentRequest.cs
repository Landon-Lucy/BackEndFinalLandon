using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Back_EndAPI.Models;

public class ReceiveShipmentRequest
{
    [Required]
    public int ShipmentId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one item must be provided.")]
    public List<ReceiveShipmentItemDto> Items { get; set; } = new();
}
