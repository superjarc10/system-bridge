namespace SystemBridge.Api.Models;

public class Shipment
{
    public int Id { get; set; }
    public string? ShipmentNumber { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ShippedAt { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public ShipmentType Type { get; set; } = ShipmentType.OUT;
    public string Color { get; set; } = "#4caf50";
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Planned;
    public bool TruckDeliveryConfirmed { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<ShipmentItem> ShipmentItems { get; set; } = new List<ShipmentItem>();
}