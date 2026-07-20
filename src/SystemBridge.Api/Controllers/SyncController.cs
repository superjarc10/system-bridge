using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBridge.Api.Models;
using SystemBridge.Api.Services;

namespace SystemBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SyncController : ControllerBase
{
    private readonly IInventorySyncService _inventorySyncService;

    public SyncController(IInventorySyncService inventorySyncService)
    {
        _inventorySyncService = inventorySyncService;
    }

    [HttpPost("orders")]
    public async Task<IActionResult> ReceiveOrder([FromBody] OrderSyncRequest request)
    {
        var result = await _inventorySyncService.ProcessOrder(request);
        return Ok(result);
    }

    [HttpPost("inventory")]
    public async Task<IActionResult> UpdateInventory([FromBody] InventoryUpdateRequest request)
    {
        var result = await _inventorySyncService.ApplyInventoryUpdate(request);
        return Ok(result);
    }

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventory()
    {
        var result = await _inventorySyncService.GetInventoryAsync();
        return Ok(result);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _inventorySyncService.GetOrdersAsync();
        return Ok(result);
    }
}
