namespace SystemBridge.Api.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public int QuantityOnHand { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
