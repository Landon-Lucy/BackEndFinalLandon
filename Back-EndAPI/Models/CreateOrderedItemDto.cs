using System.ComponentModel.DataAnnotations;

namespace Back_EndAPI.Models;

public class CreateOrderedItemDto
{
    [Required]
    public int SkuNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int Qty { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CostPerUnit { get; set; }
}
