namespace SystemBridge.Api.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string QrCode { get; set; } = string.Empty;
    public int ProductPackagingId { get; set; }
    public int? PalletId { get; set; }
    public int Quantity { get; set; } = 1;
    public bool Confirmed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Product Product { get; set; } = null!;
    public ProductPackaging ProductPackaging { get; set; } = null!;
    public Pallet? Pallet { get; set; }
}
