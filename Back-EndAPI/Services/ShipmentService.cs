using Back_EndAPI.Data;
using Back_EndAPI.Entities;
using Back_EndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Services;

public class ShipmentService
{
    private readonly AppDbContext _context;

    public ShipmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, int StatusCode, string? Error, object? Details)> ReceiveShipmentAsync(ReceiveShipmentRequest request)
    {

        if (request == null)
            return (false, 400, "Request is null", null);

        if (request.Items == null || request.Items.Count == 0)
            return (false, 400, "No items provided", null);

        // Load shipment
        var shipment = await _context.ReceivedShipments
            .Include(s => s.PurchaseOrder)
                .ThenInclude(po => po.OrderedItems)
            .Include(s => s.ReceivedItems)
            .FirstOrDefaultAsync(s => s.Id == request.ShipmentId);

        if (shipment == null)
            return (false, 404, "Shipment not found.", null);

        // Shipment must not already be received
        if (shipment.ReceivedItems != null && shipment.ReceivedItems.Any())
            return (false, 409, "Shipment has already been received.", null);

        var discrepancies = new List<object>();

        using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var itm in request.Items)
            {
                if (itm.Qty <= 0)
                {
                    await tx.RollbackAsync();
                    return (false, 400, $"Invalid quantity for SKU {itm.SkuNumber}", null);
                }

                // Optionally compare to expected qty from purchase order
                int? expectedQty = null;
                if (shipment.PurchaseOrder != null)
                {
                    var ordered = shipment.PurchaseOrder.OrderedItems
                        .FirstOrDefault(oi => oi.SkuNumber == itm.SkuNumber);
                    if (ordered != null)
                        expectedQty = ordered.Qty;

                    if (expectedQty.HasValue && expectedQty.Value != itm.Qty)
                    {
                        discrepancies.Add(new { Sku = itm.SkuNumber, Expected = expectedQty.Value, Received = itm.Qty });
                    }
                }

                // Update inventory: add to an existing bin (do NOT create new bins)
                // First try to find a bin already assigned to this SKU.
                var bin = await _context.Bins.FirstOrDefaultAsync(b => b.SkuNumber == itm.SkuNumber);

                if (bin == null)
                {
                    // If none found, try to find an empty bin (qtystored == 0) and reuse it for this SKU.
                    bin = await _context.Bins.FirstOrDefaultAsync(b => (b.Qtystored ?? 0) == 0);
                    if (bin == null)
                    {
                        await tx.RollbackAsync();
                        return (false, 400, $"No existing bin for SKU {itm.SkuNumber}. Creating new bins is not allowed and no empty bin available.", null);
                    }

                    // Assign the SKU to the empty bin and set the stored quantity
                    bin.SkuNumber = itm.SkuNumber;
                    bin.Qtystored = itm.Qty;
                    _context.Bins.Update(bin);
                }
                else
                {
                    // Bin exists for this SKU; increment its quantity
                    bin.Qtystored = (bin.Qtystored ?? 0) + itm.Qty;
                    _context.Bins.Update(bin);
                }

                // Create ReceivedItem record (only after inventory update check passes)
                var receivedItem = new ReceivedItem
                {
                    SkuNumber = itm.SkuNumber,
                    ShipmentId = shipment.Id,
                    Qty = itm.Qty
                };

                _context.ReceivedItems.Add(receivedItem);
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            var details = new { ShipmentId = shipment.Id, Discrepancies = discrepancies };
            return (true, 200, null, details);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return (false, 500, ex.Message, null);
        }
    }
}
