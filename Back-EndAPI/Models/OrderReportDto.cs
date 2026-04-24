using System;
using System.Collections.Generic;

namespace Back_EndAPI.Models;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class BoxDto
{
    public int Tracking { get; set; }
    public DateTime? DateShipped { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderReportDto
{
    public int OrderId { get; set; }
    public int? CustomerId { get; set; }
    public DateTime? DateOrdered { get; set; }
    public string? Status { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public List<BoxDto> Boxes { get; set; } = new();
}
