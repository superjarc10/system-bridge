namespace SystemBridge.Api.Models;

public class ProductPackaging
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int? PackagingTypeId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Pieces { get; set; }

    public Product Product { get; set; } = null!;
    public PackagingType? PackagingType { get; set; }
    public ICollection<ShipmentItem> ShipmentItems { get; set; } = new List<ShipmentItem>();
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}