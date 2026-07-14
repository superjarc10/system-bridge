namespace SystemBridge.Api.Models;

public class InventoryUpdateRequest
{
    public string ProductSku { get; set; } = string.Empty;
    public int QuantityDelta { get; set; }
    public string Source { get; set; } = string.Empty;
}
