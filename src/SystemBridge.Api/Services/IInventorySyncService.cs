using SystemBridge.Api.Models;

namespace SystemBridge.Api.Services;

public interface IInventorySyncService
{
    object ProcessOrder(OrderSyncRequest request);
    object ApplyInventoryUpdate(InventoryUpdateRequest request);
}
