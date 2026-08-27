using System.Text.Json.Serialization;

namespace SystemBridge.Client.Models;

public enum OrderStatus
{
    NotStarted,
    Planned,
    Manufacturing,
    Completed,
    Shipped,
    Cancelled
}

public enum ShipmentStatus
{
    Planned,
    Packing,
    Ready,
    Shipped,
    Cancelled
}

public enum ShipmentType
{
    IN,
    OUT
}

public sealed class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public List<Order> Orders { get; set; } = [];
    public List<Shipment> Shipments { get; set; } = [];
}

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? PricePer1000 { get; set; }
    public decimal ManufacturingCapacityPerHour { get; set; }
    public decimal WeightPer1000 { get; set; }
    public List<ProductPackaging> PackagingOptions { get; set; } = [];
    public List<OrderItem> OrderItems { get; set; } = [];
}

public sealed class ProductPackaging
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int? PackagingTypeId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Pieces { get; set; }
    public Product? Product { get; set; }
    public List<ShipmentItem> ShipmentItems { get; set; } = [];
}

public sealed class PackagingType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class Pallet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public sealed class Order
{
    public int Id { get; set; }
    public string? OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public string Color { get; set; } = "#b0b0b0";
    public OrderStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Customer? Customer { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}

public sealed class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Order? Order { get; set; }
    public Product? Product { get; set; }
    public List<ShipmentItem> ShipmentItems { get; set; } = [];
}

public sealed class Shipment
{
    public int Id { get; set; }
    public string? ShipmentNumber { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public ShipmentType Type { get; set; } = ShipmentType.OUT;
    public string Color { get; set; } = "#4caf50";
    public ShipmentStatus Status { get; set; }
    public bool TruckDeliveryConfirmed { get; set; }
    public Customer? Customer { get; set; }
    public List<ShipmentItem> ShipmentItems { get; set; } = [];
}

public sealed class ShipmentItem
{
    public int Id { get; set; }
    public int ShipmentId { get; set; }
    public int OrderItemId { get; set; }
    public int ProductPackagingId { get; set; }
    public int? PalletId { get; set; }
    public int Quantity { get; set; }
    public Shipment? Shipment { get; set; }
    public OrderItem? OrderItem { get; set; }
    public ProductPackaging? ProductPackaging { get; set; }
}

public sealed class InventoryItem
{
    public int Id { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public int QuantityOnHand { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public sealed class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

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
