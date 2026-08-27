namespace SystemBridge.Api.Models;

public class Order
{
    public int Id { get; set; }
    public string? OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledDate { get; set; }
    public string Color { get; set; } = "#b0b0b0";
    public OrderStatus Status { get; set; } = OrderStatus.NotStarted;
    public string Notes { get; set; } = string.Empty;

    public Customer Customer { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}