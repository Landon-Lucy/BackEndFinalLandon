using Back_EndAPI.Models;
using Back_EndAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_EndAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly ShipmentService _service;

    public ShipmentsController(ShipmentService service)
    {
        _service = service;
    }

    [HttpPost("receive")]
    public async Task<IActionResult> Receive([FromBody] ReceiveShipmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error, details) = await _service.ReceiveShipmentAsync(request);
        if (!success)
            return BadRequest(new { error });

        return Ok(new { message = "Shipment processed.", details });
    }
}
