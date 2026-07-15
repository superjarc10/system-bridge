namespace SystemBridge.Api.Models;

public class OrderRecord
{
    public int Id { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
