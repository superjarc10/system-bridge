namespace SystemBridge.Api.Models;

public sealed class CustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public sealed class ProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? PricePer1000 { get; set; }
    public decimal ManufacturingCapacityPerHour { get; set; }
    public decimal WeightPer1000 { get; set; }
}

public sealed class ProductPackagingRequest
{
    public int ProductId { get; set; }
    public int? PackagingTypeId { get; set; }
    public int Pieces { get; set; }
}

public sealed class PackagingTypeRequest
{
    public string Name { get; set; } = string.Empty;
}

public sealed class PalletRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public sealed class OrderRequest
{
    public string? OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public string Color { get; set; } = "#b0b0b0";
    public OrderStatus Status { get; set; } = OrderStatus.NotStarted;
    public string Notes { get; set; } = string.Empty;
}

public sealed class OrderItemRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public sealed class ShipmentRequest
{
    public string? ShipmentNumber { get; set; }
    public int CustomerId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public ShipmentType Type { get; set; } = ShipmentType.OUT;
    public string Color { get; set; } = "#4caf50";
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Planned;
    public bool TruckDeliveryConfirmed { get; set; }
}

public sealed class ShipmentItemRequest
{
    public int ShipmentId { get; set; }
    public int OrderItemId { get; set; }
    public int ProductPackagingId { get; set; }
    public int? PalletId { get; set; }
    public int Quantity { get; set; }
}

public sealed class InventoryItemRequest
{
    public int ProductId { get; set; }
    public string QrCode { get; set; } = string.Empty;
    public int ProductPackagingId { get; set; }
    public int? PalletId { get; set; }
    public int Quantity { get; set; } = 1;
    public bool Confirmed { get; set; }
}