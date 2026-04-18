using Back_EndAPI.Data;
using Back_EndAPI.Entities;
using Back_EndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_EndAPI.Services;

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, int StatusCode, string? Error, object? Result)> CreateOrderAsync(CreateOrderRequest request)
    {
        if (request == null)
            return (false, 400, "Request is null", null);

        if (request.Items == null || request.Items.Count == 0)
            return (false, 400, "Order must contain at least one item", null);

        // ensure customer exists
        var customer = await _context.Customers.FindAsync(request.CustomerId);
        if (customer == null)
            return (false, 404, "Customer not found", null);

        var order = new CustomerOrder
        {
            CustomerId = request.CustomerId,
            DateTimeOrdered = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        foreach (var it in request.Items)
        {
            if (it.Quantity <= 0)
                return (false, 400, $"Invalid quantity for product {it.ProductId}", null);

            var item = await _context.Items.FindAsync(it.ProductId);
            if (item == null)
                return (false, 400, $"Item with SKU {it.ProductId} does not exist", null);

            var sold = new SoldItem
            {
                SkuNumber = it.ProductId,
                Qty = it.Quantity
            };

            order.SoldItems.Add(sold);
        }

        using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.CustomerOrders.Add(order);
            await _context.SaveChangesAsync();

            // create status record
            var status = new CustomerOrderStatus
            {
                CustomerOrderId = order.Id,
                Status = "CREATED"
            };
            _context.CustomerOrderStatuses.Add(status);
            await _context.SaveChangesAsync();

            await tx.CommitAsync();

            return (true, 201, null, new { orderId = order.Id });
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return (false, 500, ex.Message, null);
        }
    }

    public async Task<(bool Success, int StatusCode, string? Error)> PickOrderAsync(int orderId, int binId)
    {
        // Load order and status
        var order = await _context.CustomerOrders
            .Include(o => o.SoldItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            return (false, 404, "Order not found");

        var status = await _context.CustomerOrderStatuses.FindAsync(orderId);
        if (status == null)
            return (false, 400, "Order status not set");

        if (status.Status == "PICKED")
            return (true, 200, null); // idempotent

        if (status.Status != "CREATED")
            return (false, 409, "Order cannot be picked in its current state");

        // Load bin
        var bin = await _context.Bins.FindAsync(binId);
        if (bin == null)
            return (false, 404, "Bin not found");

        // Picking must use a single bin only. Ensure bin contains sufficient qty for all items of that SKU.
        using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var si in order.SoldItems)
            {
                if (si.SkuNumber == null)
                    return (false, 400, "Invalid sold item SKU");

                if (bin.SkuNumber != si.SkuNumber)
                    return (false, 409, $"Bin {binId} does not store SKU {si.SkuNumber}");

                var stored = bin.Qtystored ?? 0;
                if (stored < si.Qty)
                    return (false, 409, $"Insufficient inventory in bin {binId} for SKU {si.SkuNumber}");

                // reduce inventory
                bin.Qtystored = stored - si.Qty;
                _context.Bins.Update(bin);
            }

            // set status to PICKED
            status.Status = "PICKED";
            _context.CustomerOrderStatuses.Update(status);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return (true, 200, null);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return (false, 500, ex.Message);
        }
    }

    public async Task<(bool Success, int StatusCode, string? Error)> PackOrderAsync(int orderId)
    {
        var order = await _context.CustomerOrders.FindAsync(orderId);
        if (order == null)
            return (false, 404, "Order not found");

        var status = await _context.CustomerOrderStatuses.FindAsync(orderId);
        if (status == null)
            return (false, 400, "Order status not set");

        if (status.Status == "PACKED")
            return (true, 200, null); // idempotent

        if (status.Status != "PICKED")
            return (false, 409, "Order must be PICKED before packing");

        status.Status = "PACKED";
        _context.CustomerOrderStatuses.Update(status);
        await _context.SaveChangesAsync();

        return (true, 200, null);
    }

    public async Task<(bool Success, int StatusCode, string? Error)> ShipOrderAsync(int orderId)
    {
        var order = await _context.CustomerOrders.FindAsync(orderId);
        if (order == null)
            return (false, 404, "Order not found");

        var status = await _context.CustomerOrderStatuses.FindAsync(orderId);
        if (status == null)
            return (false, 400, "Order status not set");

        if (status.Status == "SHIPPED")
            return (true, 200, null); // idempotent

        if (status.Status != "PACKED")
            return (false, 409, "Order must be PACKED before shipping");

        // create box and shipped items
        using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            var box = new Box
            {
                CustomerOrderId = orderId,
                DateShipped = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();

            // add shipped items from sold items
            var soldItems = await _context.SoldItems.Where(si => si.CustomerOrderId == orderId).ToListAsync();
            foreach (var si in soldItems)
            {
                var shipped = new ShippedItem
                {
                    BoxTracking = box.Tracking,
                    SkuNumber = si.SkuNumber,
                    Qty = si.Qty
                };
                _context.ShippedItems.Add(shipped);
            }

            status.Status = "SHIPPED";
            _context.CustomerOrderStatuses.Update(status);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return (true, 200, null);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return (false, 500, ex.Message);
        }
    }
}
