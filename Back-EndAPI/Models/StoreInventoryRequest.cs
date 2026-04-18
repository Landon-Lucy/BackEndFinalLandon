using System.ComponentModel.DataAnnotations;

namespace Back_EndAPI.Models;

public class StoreInventoryRequest
{
    [Required]
    public int BinId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
