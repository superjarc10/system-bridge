using SystemBridge.Api.Models;

namespace SystemBridge.Api.Services;

public class InventorySyncService : IInventorySyncService
{
    public object ProcessOrder(OrderSyncRequest request)
    {
        return new
        {
            message = "Order received and queued for inventory sync",
            orderId = request.OrderId,
            productSku = request.ProductSku,
            quantity = request.Quantity,
            status = request.Status
        };
    }

    public object ApplyInventoryUpdate(InventoryUpdateRequest request)
    {
        return new
        {
            message = "Inventory update accepted",
            productSku = request.ProductSku,
            quantityDelta = request.QuantityDelta,
            source = request.Source
        };
    }
}
