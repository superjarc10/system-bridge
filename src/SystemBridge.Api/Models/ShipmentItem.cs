namespace SystemBridge.Api.Models;

public class ShipmentItem
{
    public int Id { get; set; }
    public int ShipmentId { get; set; }
    public int OrderItemId { get; set; }
    public int ProductPackagingId { get; set; }
    public int? PalletId { get; set; }
    public int Quantity { get; set; }

    public Shipment Shipment { get; set; } = null!;
    public OrderItem OrderItem { get; set; } = null!;
    public ProductPackaging ProductPackaging { get; set; } = null!;
    public Pallet? Pallet { get; set; }
}