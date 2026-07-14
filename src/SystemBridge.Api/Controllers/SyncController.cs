using Microsoft.AspNetCore.Mvc;
using SystemBridge.Api.Models;
using SystemBridge.Api.Services;

namespace SystemBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly IInventorySyncService _inventorySyncService;

    public SyncController(IInventorySyncService inventorySyncService)
    {
        _inventorySyncService = inventorySyncService;
    }

    [HttpPost("orders")]
    public IActionResult ReceiveOrder([FromBody] OrderSyncRequest request)
    {
        var result = _inventorySyncService.ProcessOrder(request);
        return Ok(result);
    }

    [HttpPost("inventory")]
    public IActionResult UpdateInventory([FromBody] InventoryUpdateRequest request)
    {
        var result = _inventorySyncService.ApplyInventoryUpdate(request);
        return Ok(result);
    }
}
