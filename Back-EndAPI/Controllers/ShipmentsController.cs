using Back_EndAPI.Models;
using Back_EndAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_EndAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShipmentsController : ControllerBase
{
    private readonly ShipmentService _service;

    public ShipmentsController(ShipmentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShipmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, statusCode, error, details) = await _service.CreateShipmentAsync(request);
        if (!success)
        {
            return statusCode switch
            {
                400 => BadRequest(new { error }),
                404 => NotFound(new { error }),
                500 => StatusCode(500, new { error }),
                _ => BadRequest(new { error })
            };
        }

        return Created($"/api/shipments/{details}", new { id = details });
    }

    [HttpPost("{id}/receive")]
    public async Task<IActionResult> Receive(int id, [FromBody] ReceiveShipmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // ensure route id matches payload
        if (request.ShipmentId != id)
            return BadRequest(new { error = "Shipment id mismatch" });

        var (success, statusCode, error, details) = await _service.ReceiveShipmentAsync(request);
        if (!success)
        {
            return statusCode switch
            {
                404 => NotFound(new { error }),
                409 => Conflict(new { error }),
                400 => BadRequest(new { error }),
                500 => StatusCode(500, new { error }),
                _ => BadRequest(new { error })
            };
        }

        return Ok(new { message = "Shipment processed.", details });
    }
}
