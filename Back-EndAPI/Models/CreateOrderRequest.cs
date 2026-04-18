using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Back_EndAPI.Models;

public class CreateOrderRequest
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreateOrderItemDto> Items { get; set; } = new();
}
