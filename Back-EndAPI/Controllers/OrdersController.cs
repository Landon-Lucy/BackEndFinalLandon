using Back_EndAPI.Models;
using Back_EndAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_EndAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, statusCode, error, result) = await _service.CreateOrderAsync(request);
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

        return Created($"/api/orders/{((dynamic)result).orderId}", result);
    }

    [HttpPost("{id}/pick")]
    public async Task<IActionResult> Pick(int id, [FromQuery] int binId)
    {
        var (success, statusCode, error) = await _service.PickOrderAsync(id, binId);
        if (!success)
        {
            return statusCode switch
            {
                400 => BadRequest(new { error }),
                404 => NotFound(new { error }),
                409 => Conflict(new { error }),
                500 => StatusCode(500, new { error }),
                _ => BadRequest(new { error })
            };
        }

        return Ok(new { message = "Order picked." });
    }

    [HttpPost("{id}/pack")]
    public async Task<IActionResult> Pack(int id)
    {
        var (success, statusCode, error) = await _service.PackOrderAsync(id);
        if (!success)
        {
            return statusCode switch
            {
                400 => BadRequest(new { error }),
                404 => NotFound(new { error }),
                409 => Conflict(new { error }),
                500 => StatusCode(500, new { error }),
                _ => BadRequest(new { error })
            };
        }

        return Ok(new { message = "Order packed." });
    }

    [HttpPost("{id}/ship")]
    public async Task<IActionResult> Ship(int id)
    {
        var (success, statusCode, error) = await _service.ShipOrderAsync(id);
        if (!success)
        {
            return statusCode switch
            {
                400 => BadRequest(new { error }),
                404 => NotFound(new { error }),
                409 => Conflict(new { error }),
                500 => StatusCode(500, new { error }),
                _ => BadRequest(new { error })
            };
        }

        return Ok(new { message = "Order shipped." });
    }
}
