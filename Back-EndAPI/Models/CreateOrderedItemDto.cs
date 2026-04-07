using System.ComponentModel.DataAnnotations;

namespace Back_EndAPI.Models;

public class CreateOrderedItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CostPerUnit { get; set; }
}
