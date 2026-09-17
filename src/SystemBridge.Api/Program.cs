using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SystemBridge.Api.Data;
using SystemBridge.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
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
        // Accepts loopback and private LAN origins so other devices can reach this API during testing.
        policy.SetIsOriginAllowed(origin =>
                Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
                (uri.IsLoopback ||
                 (System.Net.IPAddress.TryParse(uri.Host, out var ip) && IsPrivateNetworkAddress(ip))))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

static bool IsPrivateNetworkAddress(System.Net.IPAddress ip)
{
    var bytes = ip.GetAddressBytes();
    if (bytes.Length != 4) return false;
    return bytes[0] == 10 ||
        (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
        (bytes[0] == 192 && bytes[1] == 168);
}
builder.Services.AddDbContext<SystemBridgeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SystemBridgeDbContext>();
    db.Database.Migrate();

    if (!db.PackagingTypes.Any())
    {
        db.PackagingTypes.AddRange(
            new PackagingType { Name = "Box" },
            new PackagingType { Name = "Bag" },
            new PackagingType { Name = "Crate" }
        );
    }

    db.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SystemBridge API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowLocalhost");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
