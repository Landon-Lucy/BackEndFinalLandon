using Back_EndAPI.Data;
using Back_EndAPI.Entities;
using Back_EndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Services;

public class PurchaseOrderService
{
    private readonly AppDbContext _context;

    public PurchaseOrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrder?> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request)
    {
        // Basic validation
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Items == null || request.Items.Count == 0)
            throw new ArgumentException("At least one ordered item is required.");

        // Ensure vendor exists
        var vendor = await _context.Vendors.FindAsync(request.VendorId);
        if (vendor == null)
            return null;

        var po = new PurchaseOrder
        {
            DateOrdered = DateOnly.FromDateTime(request.DateOrdered),
            Vendorid = request.VendorId,
            ExpectedTotalCost = request.ExpectedTotalCost
        };

        foreach (var it in request.Items)
        {
            if (it.Qty <= 0)
                throw new ArgumentException($"Invalid quantity for SKU {it.SkuNumber}");

            var item = await _context.Items.FindAsync(it.SkuNumber);
            if (item == null)
                throw new ArgumentException($"Item with SKU {it.SkuNumber} does not exist.");

            var ordered = new OrderedItem
            {
                SkuNumber = it.SkuNumber,
                Qty = it.Qty,
                CostPerUnit = it.CostPerUnit
            };

            po.OrderedItems.Add(ordered);
        }

        _context.PurchaseOrders.Add(po);
        await _context.SaveChangesAsync();

        return po;
    }
}
