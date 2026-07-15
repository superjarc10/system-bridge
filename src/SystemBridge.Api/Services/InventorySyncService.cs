using Microsoft.EntityFrameworkCore;
using SystemBridge.Api.Data;
using SystemBridge.Api.Models;

namespace SystemBridge.Api.Services;

public class InventorySyncService : IInventorySyncService
{
    private readonly SystemBridgeDbContext _dbContext;

    public InventorySyncService(SystemBridgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<object> ProcessOrder(OrderSyncRequest request)
    {
        var order = new OrderRecord
        {
            OrderId = request.OrderId,
            ProductSku = request.ProductSku,
            Quantity = request.Quantity,
            Status = request.Status
        };

        _dbContext.OrderRecords.Add(order);

        var inventoryItem = await _dbContext.InventoryItems
            .FirstOrDefaultAsync(x => x.ProductSku == request.ProductSku);

        if (inventoryItem is null)
        {
            inventoryItem = new InventoryItem
            {
                ProductSku = request.ProductSku,
                QuantityOnHand = 0
            };
            _dbContext.InventoryItems.Add(inventoryItem);
        }

        inventoryItem.QuantityOnHand -= request.Quantity;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new
        {
            message = "Order received and inventory updated",
            orderId = order.OrderId,
            productSku = order.ProductSku,
            quantity = order.Quantity,
            status = order.Status,
            remainingQuantity = inventoryItem.QuantityOnHand
        };
    }

    public async Task<object> ApplyInventoryUpdate(InventoryUpdateRequest request)
    {
        var inventoryItem = await _dbContext.InventoryItems
            .FirstOrDefaultAsync(x => x.ProductSku == request.ProductSku);

        if (inventoryItem is null)
        {
            inventoryItem = new InventoryItem
            {
                ProductSku = request.ProductSku,
                QuantityOnHand = 0
            };
            _dbContext.InventoryItems.Add(inventoryItem);
        }

        inventoryItem.QuantityOnHand += request.QuantityDelta;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new
        {
            message = "Inventory update accepted",
            productSku = request.ProductSku,
            quantityDelta = request.QuantityDelta,
            source = request.Source,
            newQuantity = inventoryItem.QuantityOnHand
        };
    }

    public async Task<List<InventoryItem>> GetInventoryAsync()
    {
        return await _dbContext.InventoryItems
            .OrderBy(x => x.ProductSku)
            .ToListAsync();
    }

    public async Task<List<OrderRecord>> GetOrdersAsync()
    {
        return await _dbContext.OrderRecords
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
