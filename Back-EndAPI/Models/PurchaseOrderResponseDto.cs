using System;
using System.Collections.Generic;

namespace Back_EndAPI.Models;

public class OrderedItemResponseDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class PurchaseOrderResponseDto
{
    public int Id { get; set; }
    public DateTime DateOrdered { get; set; }
    public int? VendorId { get; set; }
    public List<OrderedItemResponseDto> Items { get; set; } = new();
}
