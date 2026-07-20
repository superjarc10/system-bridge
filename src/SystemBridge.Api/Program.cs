using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SystemBridge.Api.Data;
using SystemBridge.Api.Models;
using SystemBridge.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "super-secret-key-for-demo-123456")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:8000", "http://localhost:8000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<SystemBridgeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=systembridge.db"));
builder.Services.AddScoped<IInventorySyncService, InventorySyncService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SystemBridgeDbContext>();
    db.Database.EnsureCreated();

    if (!db.InventoryItems.Any())
    {
        db.InventoryItems.AddRange(
            new InventoryItem { ProductSku = "SKU-001", QuantityOnHand = 25, UpdatedAt = DateTime.UtcNow },
            new InventoryItem { ProductSku = "SKU-002", QuantityOnHand = 10, UpdatedAt = DateTime.UtcNow },
            new InventoryItem { ProductSku = "SKU-003", QuantityOnHand = 40, UpdatedAt = DateTime.UtcNow }
        );
    }

    if (!db.OrderRecords.Any())
    {
        db.OrderRecords.AddRange(
            new OrderRecord { OrderId = "ORD-1001", ProductSku = "SKU-001", Quantity = 3, Status = "Pending", CreatedAt = DateTime.UtcNow },
            new OrderRecord { OrderId = "ORD-1002", ProductSku = "SKU-002", Quantity = 2, Status = "Completed", CreatedAt = DateTime.UtcNow }
        );
    }

    db.SaveChanges();
}

app.UseCors("AllowLocalhost");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
