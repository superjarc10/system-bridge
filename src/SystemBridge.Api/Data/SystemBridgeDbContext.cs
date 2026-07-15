using Microsoft.EntityFrameworkCore;
using SystemBridge.Api.Models;

namespace SystemBridge.Api.Data;

public class SystemBridgeDbContext : DbContext
{
    public SystemBridgeDbContext(DbContextOptions<SystemBridgeDbContext> options) : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<OrderRecord> OrderRecords => Set<OrderRecord>();
}
