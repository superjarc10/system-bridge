namespace SystemBridge.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? PricePer1000 { get; set; }
    public decimal ManufacturingCapacityPerHour { get; set; }
    public decimal WeightPer1000 { get; set; }

    public ICollection<ProductPackaging> PackagingOptions { get; set; } = new List<ProductPackaging>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}