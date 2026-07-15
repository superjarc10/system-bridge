using SystemBridge.Api.Models;

namespace SystemBridge.Api.Services;

public interface IInventorySyncService
{
    Task<object> ProcessOrder(OrderSyncRequest request);
    Task<object> ApplyInventoryUpdate(InventoryUpdateRequest request);
    Task<List<InventoryItem>> GetInventoryAsync();
    Task<List<OrderRecord>> GetOrdersAsync();
}
