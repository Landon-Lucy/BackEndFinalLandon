using Back_EndAPI.Models;
using Back_EndAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_EndAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrdersController : ControllerBase
{
    private readonly PurchaseOrderService _service;

    public PurchaseOrdersController(PurchaseOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var po = await _service.CreatePurchaseOrderAsync(request);
            if (po == null)
                return BadRequest(new { error = "Vendor not found." });

            // map to response DTO
            var dto = new PurchaseOrderResponseDto
            {
                Id = po.Id,
                DateOrdered = po.DateOrdered.ToDateTime(TimeOnly.MinValue),
                VendorId = po.Vendorid
            };

            foreach (var oi in po.OrderedItems)
            {
                dto.Items.Add(new OrderedItemResponseDto
                {
                    Id = oi.Id,
                    ProductId = oi.SkuNumber ?? 0,
                    Quantity = oi.Qty
                });
            }

            return Created($"/api/purchaseorders/{po.Id}", dto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
