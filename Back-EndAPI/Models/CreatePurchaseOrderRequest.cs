using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Back_EndAPI.Models;

public class CreatePurchaseOrderRequest
{
    [Required]
    public DateTime DateOrdered { get; set; }

    [Required]
    public int VendorId { get; set; }

    [Range(0, int.MaxValue)]
    public int? ExpectedTotalCost { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreateOrderedItemDto> Items { get; set; } = new();
}
