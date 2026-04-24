using Back_EndAPI.Data;
using Back_EndAPI.Entities;
using Back_EndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Services;

public class InventoryService
{
    private readonly AppDbContext _context;

    public InventoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, int StatusCode, string? Error)> StoreAsync(StoreInventoryRequest request)
    {
        if (request == null)
            return (false, 400, "Request is null");

        if (request.Quantity < 0)
            return (false, 400, "Quantity cannot be negative");

        var bin = await _context.Bins.FindAsync(request.BinId);
        if (bin == null)
            return (false, 404, "Bin not found");

        var item = await _context.Items.FindAsync(request.ProductId);
        if (item == null)
            return (false, 404, "Item not found");

        // A bin stores only one product. If bin has an assigned SKU, it must match.
        if (bin.SkuNumber.HasValue && bin.SkuNumber != request.ProductId)
            return (false, 409, "Bin already stores a different SKU");

        // Assign SKU if empty and update quantity
        bin.SkuNumber = request.ProductId;
        bin.Qtystored = (bin.Qtystored ?? 0) + request.Quantity;
        if (bin.Qtystored < 0)
            return (false, 400, "Resulting inventory cannot be negative");

        _context.Bins.Update(bin);
        await _context.SaveChangesAsync();

        return (true, 200, null);
    }

    public async Task<(bool Success, int StatusCode, string? Error, List<InventoryReportDto>? Result)> GetInventoryAsync(int? productId = null)
    {
        // If productId provided, ensure it exists
        if (productId.HasValue)
        {
            var item = await _context.Items.FindAsync(productId.Value);
            if (item == null)
                return (false, 404, "Product not found", null);
        }

        var binsQuery = _context.Bins.AsQueryable();
        if (productId.HasValue)
            binsQuery = binsQuery.Where(b => b.SkuNumber == productId.Value);

        // Only consider bins that have an assigned SKU
        var bins = await binsQuery.Where(b => b.SkuNumber != null).ToListAsync();

        var grouped = bins.GroupBy(b => b.SkuNumber);

        var result = new List<InventoryReportDto>();
        foreach (var g in grouped)
        {
            var pid = g.Key ?? 0;
            var item = await _context.Items.FindAsync(pid);
            var dto = new InventoryReportDto
            {
                ProductId = pid,
                ProductName = item?.Name,
                TotalQuantity = g.Sum(b => b.Qtystored ?? 0)
            };

            foreach (var b in g)
            {
                dto.Bins.Add(new BinSummaryDto
                {
                    BinId = b.Id,
                    ProductId = b.SkuNumber,
                    Quantity = b.Qtystored ?? 0
                });
            }

            result.Add(dto);
        }

        return (true, 200, null, result);
    }
}
