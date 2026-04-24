using System;
using System.Collections.Generic;

namespace Back_EndAPI.Models;

public class BinSummaryDto
{
    public int BinId { get; set; }
    public int? ProductId { get; set; }
    public int Quantity { get; set; }
}

public class InventoryReportDto
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int TotalQuantity { get; set; }
    public List<BinSummaryDto> Bins { get; set; } = new();
}
