using Microsoft.EntityFrameworkCore;
using SystemBridge.Api.Models;

namespace SystemBridge.Api.Data;

public class SystemBridgeDbContext : DbContext
{
    public SystemBridgeDbContext(DbContextOptions<SystemBridgeDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductPackaging> ProductPackagings => Set<ProductPackaging>();
    public DbSet<PackagingType> PackagingTypes => Set<PackagingType>();
    public DbSet<Pallet> Pallets => Set<Pallet>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<ShipmentItem> ShipmentItems => Set<ShipmentItem>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<ChangeHistory> ChangeHistory => Set<ChangeHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(order => order.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Shipment>()
            .Property(shipment => shipment.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Shipment>()
            .Property(shipment => shipment.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
            .HasIndex(order => order.OrderNumber)
            .IsUnique();

        modelBuilder.Entity<Shipment>()
            .HasIndex(shipment => shipment.ShipmentNumber)
            .IsUnique();

        modelBuilder.Entity<ProductPackaging>()
            .HasOne(packaging => packaging.Product)
            .WithMany(product => product.PackagingOptions)
            .HasForeignKey(packaging => packaging.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PackagingType>()
            .HasIndex(type => type.Name)
            .IsUnique();

        modelBuilder.Entity<ProductPackaging>()
            .HasOne(packaging => packaging.PackagingType)
            .WithMany(type => type.Packagings)
            .HasForeignKey(packaging => packaging.PackagingTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(order => order.Customer)
            .WithMany(customer => customer.Orders)
            .HasForeignKey(order => order.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(item => item.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(item => item.Product)
            .WithMany(product => product.OrderItems)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Shipment>()
            .HasOne(shipment => shipment.Customer)
            .WithMany(customer => customer.Shipments)
            .HasForeignKey(shipment => shipment.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShipmentItem>()
            .HasOne(item => item.Shipment)
            .WithMany(shipment => shipment.ShipmentItems)
            .HasForeignKey(item => item.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShipmentItem>()
            .HasOne(item => item.OrderItem)
            .WithMany(orderItem => orderItem.ShipmentItems)
            .HasForeignKey(item => item.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShipmentItem>()
            .HasOne(item => item.ProductPackaging)
            .WithMany(packaging => packaging.ShipmentItems)
            .HasForeignKey(item => item.ProductPackagingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShipmentItem>()
            .HasOne(item => item.Pallet)
            .WithMany()
            .HasForeignKey(item => item.PalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(item => item.Product)
            .WithMany(product => product.InventoryItems)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(item => item.ProductPackaging)
            .WithMany(packaging => packaging.InventoryItems)
            .HasForeignKey(item => item.ProductPackagingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(item => item.Pallet)
            .WithMany()
            .HasForeignKey(item => item.PalletId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
