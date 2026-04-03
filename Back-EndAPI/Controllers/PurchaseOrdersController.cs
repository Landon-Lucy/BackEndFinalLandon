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

            return Created($"/api/purchaseorders/{po.Id}", po);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
