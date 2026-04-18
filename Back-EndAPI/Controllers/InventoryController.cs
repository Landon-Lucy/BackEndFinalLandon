using Back_EndAPI.Models;
using Back_EndAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_EndAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _service;

    public InventoryController(InventoryService service)
    {
        _service = service;
    }

    [HttpPost("store")]
    public async Task<IActionResult> Store([FromBody] StoreInventoryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, statusCode, error) = await _service.StoreAsync(request);
        if (!success)
        {
            return statusCode switch
            {
                400 => BadRequest(new { error }),
                404 => NotFound(new { error }),
                409 => Conflict(new { error }),
                _ => BadRequest(new { error })
            };
        }

        return Ok(new { message = "Inventory stored." });
    }
}
