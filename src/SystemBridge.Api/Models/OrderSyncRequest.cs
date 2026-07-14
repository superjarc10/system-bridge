namespace SystemBridge.Api.Models;

public class OrderSyncRequest
{
    public string OrderId { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Status { get; set; } = "Pending";
}
