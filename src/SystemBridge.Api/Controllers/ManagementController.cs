using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemBridge.Api.Data;
using SystemBridge.Api.Models;

namespace SystemBridge.Api.Controllers;

[ApiController]
[Authorize]
public class ManagementController(SystemBridgeDbContext db) : ControllerBase
{
    [HttpGet("api/customers")]
    public Task<List<Customer>> GetCustomers() => db.Customers.AsNoTracking().ToListAsync();

    [HttpGet("api/customers/{id:int}")]
    public Task<Customer?> GetCustomer(int id) => db.Customers.AsNoTracking().FirstOrDefaultAsync(customer => customer.Id == id);

    [HttpPost("api/customers")]
    public async Task<ActionResult<Customer>> AddCustomer(CustomerRequest request)
    {
        var customer = new Customer { Name = request.Name, Country = request.Country };
        db.Customers.Add(customer);
        await db.SaveChangesAsync();
        return Created($"api/customers/{customer.Id}", customer);
    }

    [HttpPut("api/customers/{id:int}")]
    public Task<IActionResult> UpdateCustomer(int id, CustomerRequest request) => UpdateAsync(
        db.Customers, id, customer => { customer.Name = request.Name; customer.Country = request.Country; });

    [HttpDelete("api/customers/{id:int}")]
    public Task<IActionResult> RemoveCustomer(int id) => RemoveAsync(db.Customers, id);

    [HttpPost("api/products")]
    public async Task<ActionResult<Product>> AddProduct(ProductRequest request)
    {
        var product = new Product();
        Apply(product, request);
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return Created($"api/products/{product.Id}", product);
    }

    [HttpGet("api/products")]
    public Task<List<Product>> GetProducts() => db.Products.AsNoTracking().ToListAsync();

    [HttpGet("api/products/{id:int}")]
    public Task<Product?> GetProduct(int id) => db.Products.AsNoTracking().FirstOrDefaultAsync(product => product.Id == id);

    [HttpPut("api/products/{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductRequest request)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();
        Apply(product, request);
        await db.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("api/products/{id:int}")]
    public Task<IActionResult> RemoveProduct(int id) => RemoveAsync(db.Products, id);

    [HttpPost("api/product-packaging")]
    public async Task<ActionResult<ProductPackaging>> AddProductPackaging(ProductPackagingRequest request)
    {
        if (!await db.Products.AnyAsync(product => product.Id == request.ProductId)) return NotFound("Product not found.");
        var packagingType = request.PackagingTypeId.HasValue
            ? await db.PackagingTypes.FindAsync(request.PackagingTypeId.Value)
            : null;
        if (request.PackagingTypeId.HasValue && packagingType is null) return NotFound("Packaging type not found.");
        var packaging = new ProductPackaging { ProductId = request.ProductId, PackagingTypeId = request.PackagingTypeId, Type = packagingType?.Name ?? string.Empty, Pieces = request.Pieces };
        db.ProductPackagings.Add(packaging);
        await db.SaveChangesAsync();
        return Created($"api/product-packaging/{packaging.Id}", packaging);
    }

    [HttpGet("api/product-packaging")]
    public Task<List<ProductPackaging>> GetProductPackaging() => db.ProductPackagings.AsNoTracking().ToListAsync();

    [HttpGet("api/product-packaging/{id:int}")]
    public Task<ProductPackaging?> GetProductPackaging(int id) => db.ProductPackagings.AsNoTracking().FirstOrDefaultAsync(packaging => packaging.Id == id);

    [HttpPut("api/product-packaging/{id:int}")]
    public async Task<IActionResult> UpdateProductPackaging(int id, ProductPackagingRequest request)
    {
        if (!await db.Products.AnyAsync(product => product.Id == request.ProductId)) return NotFound("Product not found.");
        var packagingType = request.PackagingTypeId.HasValue
            ? await db.PackagingTypes.FindAsync(request.PackagingTypeId.Value)
            : null;
        if (request.PackagingTypeId.HasValue && packagingType is null) return NotFound("Packaging type not found.");
        var packaging = await db.ProductPackagings.FindAsync(id);
        if (packaging is null) return NotFound();
        packaging.ProductId = request.ProductId;
        packaging.PackagingTypeId = request.PackagingTypeId;
        packaging.Type = packagingType?.Name ?? string.Empty;
        packaging.Pieces = request.Pieces;
        await db.SaveChangesAsync();
        return Ok(packaging);
    }

    [HttpDelete("api/product-packaging/{id:int}")]
    public Task<IActionResult> RemoveProductPackaging(int id) => RemoveAsync(db.ProductPackagings, id);

    [HttpGet("api/packaging-types")]
    public Task<List<PackagingType>> GetPackagingTypes() => db.PackagingTypes.AsNoTracking().OrderBy(type => type.Name).ToListAsync();

    [HttpGet("api/packaging-types/{id:int}")]
    public Task<PackagingType?> GetPackagingType(int id) => db.PackagingTypes.AsNoTracking().FirstOrDefaultAsync(type => type.Id == id);

    [HttpPost("api/packaging-types")]
    public async Task<ActionResult<PackagingType>> AddPackagingType(PackagingTypeRequest request)
    {
        var type = new PackagingType { Name = request.Name.Trim() };
        db.PackagingTypes.Add(type);
        await db.SaveChangesAsync();
        return Created($"api/packaging-types/{type.Id}", type);
    }

    [HttpPut("api/packaging-types/{id:int}")]
    public async Task<IActionResult> UpdatePackagingType(int id, PackagingTypeRequest request)
    {
        var type = await db.PackagingTypes.FindAsync(id);
        if (type is null) return NotFound();
        type.Name = request.Name.Trim();
        foreach (var packaging in await db.ProductPackagings.Where(packaging => packaging.PackagingTypeId == id).ToListAsync()) packaging.Type = type.Name;
        await db.SaveChangesAsync();
        return Ok(type);
    }

    [HttpDelete("api/packaging-types/{id:int}")]
    public Task<IActionResult> RemovePackagingType(int id) => RemoveAsync(db.PackagingTypes, id);

    [HttpGet("api/pallets")]
    public Task<List<Pallet>> GetPallets() => db.Pallets.AsNoTracking().ToListAsync();

    [HttpGet("api/pallets/{id:int}")]
    public Task<Pallet?> GetPallet(int id) => db.Pallets.AsNoTracking().FirstOrDefaultAsync(pallet => pallet.Id == id);

    [HttpPost("api/pallets")]
    public async Task<ActionResult<Pallet>> AddPallet(PalletRequest request)
    {
        var pallet = new Pallet { Name = request.Name, Description = request.Description, Quantity = request.Quantity };
        db.Pallets.Add(pallet);
        await db.SaveChangesAsync();
        return Created($"api/pallets/{pallet.Id}", pallet);
    }

    [HttpPut("api/pallets/{id:int}")]
    public async Task<IActionResult> UpdatePallet(int id, PalletRequest request)
    {
        var pallet = await db.Pallets.FindAsync(id);
        if (pallet is null) return NotFound();
        pallet.Name = request.Name;
        pallet.Description = request.Description;
        pallet.Quantity = request.Quantity;
        await db.SaveChangesAsync();
        return Ok(pallet);
    }

    [HttpDelete("api/pallets/{id:int}")]
    public Task<IActionResult> RemovePallet(int id) => RemoveAsync(db.Pallets, id);

    [HttpPost("api/orders")]
    public async Task<ActionResult<Order>> AddOrder(OrderRequest request)
    {
        if (!await db.Customers.AnyAsync(customer => customer.Id == request.CustomerId)) return NotFound("Customer not found.");
        var order = new Order { OrderNumber = NormalizeOrderNumber(request.OrderNumber), CustomerId = request.CustomerId, CreatedAt = ToUtc(request.CreatedAt) ?? DateTime.UtcNow, ScheduledDate = ToUtc(request.ScheduledDate), Color = request.Color, Status = request.Status, Notes = request.Notes };
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        return Created($"api/orders/{order.Id}", order);
    }

    [HttpGet("api/orders")]
    public Task<List<Order>> GetOrders() => db.Orders.AsNoTracking().ToListAsync();

    [HttpGet("api/orders/{id:int}")]
    public Task<Order?> GetOrder(int id) => db.Orders.AsNoTracking().FirstOrDefaultAsync(order => order.Id == id);

    [HttpPut("api/orders/{id:int}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderRequest request)
    {
        if (!await db.Customers.AnyAsync(customer => customer.Id == request.CustomerId)) return NotFound("Customer not found.");
        var order = await db.Orders.FindAsync(id);
        if (order is null) return NotFound();
        order.CustomerId = request.CustomerId;
        order.OrderNumber = NormalizeOrderNumber(request.OrderNumber);
        order.Status = request.Status;
        order.Notes = request.Notes;
        order.ScheduledDate = ToUtc(request.ScheduledDate);
        order.Color = request.Color;
        if (request.CreatedAt.HasValue) order.CreatedAt = ToUtc(request.CreatedAt)!.Value;
        await db.SaveChangesAsync();
        return Ok(order);
    }

    [HttpDelete("api/orders/{id:int}")]
    public Task<IActionResult> RemoveOrder(int id) => RemoveAsync(db.Orders, id);

    [HttpPost("api/order-items")]
    public async Task<ActionResult<OrderItem>> AddOrderItem(OrderItemRequest request)
    {
        if (!await db.Orders.AnyAsync(order => order.Id == request.OrderId)) return NotFound("Order not found.");
        if (!await db.Products.AnyAsync(product => product.Id == request.ProductId)) return NotFound("Product not found.");
        var item = new OrderItem { OrderId = request.OrderId, ProductId = request.ProductId, Quantity = request.Quantity };
        db.OrderItems.Add(item);
        await db.SaveChangesAsync();
        return Created($"api/order-items/{item.Id}", item);
    }

    [HttpGet("api/order-items")]
    public Task<List<OrderItem>> GetOrderItems() => db.OrderItems.AsNoTracking().ToListAsync();

    [HttpGet("api/order-items/{id:int}")]
    public Task<OrderItem?> GetOrderItem(int id) => db.OrderItems.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

    [HttpPut("api/order-items/{id:int}")]
    public async Task<IActionResult> UpdateOrderItem(int id, OrderItemRequest request)
    {
        if (!await db.Orders.AnyAsync(order => order.Id == request.OrderId)) return NotFound("Order not found.");
        if (!await db.Products.AnyAsync(product => product.Id == request.ProductId)) return NotFound("Product not found.");
        var item = await db.OrderItems.FindAsync(id);
        if (item is null) return NotFound();
        item.OrderId = request.OrderId;
        item.ProductId = request.ProductId;
        item.Quantity = request.Quantity;
        await db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("api/order-items/{id:int}")]
    public Task<IActionResult> RemoveOrderItem(int id) => RemoveAsync(db.OrderItems, id);

    [HttpPost("api/shipments")]
    public async Task<ActionResult<Shipment>> AddShipment(ShipmentRequest request)
    {
        if (!await db.Customers.AnyAsync(customer => customer.Id == request.CustomerId)) return NotFound("Customer not found.");
        var shipment = new Shipment { ShipmentNumber = NormalizeShipmentNumber(request.ShipmentNumber), CustomerId = request.CustomerId, CreatedAt = ToUtc(request.CreatedAt) ?? DateTime.UtcNow, ScheduledDate = ToUtc(request.ScheduledDate), ShippedAt = ToUtc(request.ShippedAt), Color = request.Color, Status = request.Status };
        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();
        return Created($"api/shipments/{shipment.Id}", shipment);
    }

    [HttpGet("api/shipments")]
    public Task<List<Shipment>> GetShipments() => db.Shipments.AsNoTracking().ToListAsync();

    [HttpGet("api/shipments/{id:int}")]
    public Task<Shipment?> GetShipment(int id) => db.Shipments.AsNoTracking().FirstOrDefaultAsync(shipment => shipment.Id == id);

    [HttpPut("api/shipments/{id:int}")]
    public async Task<IActionResult> UpdateShipment(int id, ShipmentRequest request)
    {
        if (!await db.Customers.AnyAsync(customer => customer.Id == request.CustomerId)) return NotFound("Customer not found.");
        var shipment = await db.Shipments.FindAsync(id);
        if (shipment is null) return NotFound();
        shipment.CustomerId = request.CustomerId;
        shipment.ShipmentNumber = NormalizeShipmentNumber(request.ShipmentNumber);
        shipment.ScheduledDate = ToUtc(request.ScheduledDate);
        shipment.ShippedAt = ToUtc(request.ShippedAt);
        shipment.Color = request.Color;
        shipment.Status = request.Status;
        if (request.CreatedAt.HasValue) shipment.CreatedAt = ToUtc(request.CreatedAt)!.Value;
        await db.SaveChangesAsync();
        return Ok(shipment);
    }

    [HttpDelete("api/shipments/{id:int}")]
    public Task<IActionResult> RemoveShipment(int id) => RemoveAsync(db.Shipments, id);

    [HttpPost("api/shipment-items")]
    public async Task<ActionResult<ShipmentItem>> AddShipmentItem(ShipmentItemRequest request)
    {
        if (!await db.Shipments.AnyAsync(shipment => shipment.Id == request.ShipmentId)) return NotFound("Shipment not found.");
        if (!await db.OrderItems.AnyAsync(item => item.Id == request.OrderItemId)) return NotFound("Order item not found.");
        if (!await db.ProductPackagings.AnyAsync(packaging => packaging.Id == request.ProductPackagingId)) return NotFound("Product packaging not found.");
        if (request.PalletId.HasValue && !await db.Pallets.AnyAsync(pallet => pallet.Id == request.PalletId.Value)) return NotFound("Pallet not found.");
        var item = new ShipmentItem { ShipmentId = request.ShipmentId, OrderItemId = request.OrderItemId, ProductPackagingId = request.ProductPackagingId, PalletId = request.PalletId, Quantity = request.Quantity };
        db.ShipmentItems.Add(item);
        await db.SaveChangesAsync();
        return Created($"api/shipment-items/{item.Id}", item);
    }

    [HttpGet("api/shipment-items")]
    public Task<List<ShipmentItem>> GetShipmentItems() => db.ShipmentItems.AsNoTracking().ToListAsync();

    [HttpGet("api/shipment-items/{id:int}")]
    public Task<ShipmentItem?> GetShipmentItem(int id) => db.ShipmentItems.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

    [HttpPut("api/shipment-items/{id:int}")]
    public async Task<IActionResult> UpdateShipmentItem(int id, ShipmentItemRequest request)
    {
        if (!await db.Shipments.AnyAsync(shipment => shipment.Id == request.ShipmentId)) return NotFound("Shipment not found.");
        if (!await db.OrderItems.AnyAsync(item => item.Id == request.OrderItemId)) return NotFound("Order item not found.");
        if (!await db.ProductPackagings.AnyAsync(packaging => packaging.Id == request.ProductPackagingId)) return NotFound("Product packaging not found.");
        if (request.PalletId.HasValue && !await db.Pallets.AnyAsync(pallet => pallet.Id == request.PalletId.Value)) return NotFound("Pallet not found.");
        var item = await db.ShipmentItems.FindAsync(id);
        if (item is null) return NotFound();
        item.ShipmentId = request.ShipmentId;
        item.OrderItemId = request.OrderItemId;
        item.ProductPackagingId = request.ProductPackagingId;
        item.PalletId = request.PalletId;
        item.Quantity = request.Quantity;
        await db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("api/shipment-items/{id:int}")]
    public Task<IActionResult> RemoveShipmentItem(int id) => RemoveAsync(db.ShipmentItems, id);

    private async Task<IActionResult> UpdateAsync<TEntity>(DbSet<TEntity> entities, int id, Action<TEntity> update)
        where TEntity : class
    {
        var entity = await entities.FindAsync(id);
        if (entity is null) return NotFound();
        update(entity);
        await db.SaveChangesAsync();
        return Ok(entity);
    }

    private async Task<IActionResult> RemoveAsync<TEntity>(DbSet<TEntity> entities, int id)
        where TEntity : class
    {
        var entity = await entities.FindAsync(id);
        if (entity is null) return NotFound();
        entities.Remove(entity);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("The record cannot be removed while it is referenced by another record.");
        }

        return NoContent();
    }

    private static void Apply(Product product, ProductRequest request)
    {
        product.Name = request.Name;
        product.Description = request.Description;
        product.PricePer1000 = request.PricePer1000;
        product.ManufacturingCapacityPerHour = request.ManufacturingCapacityPerHour;
        product.WeightPer1000 = request.WeightPer1000;
    }

    private static string? NormalizeOrderNumber(string? orderNumber) =>
        string.IsNullOrWhiteSpace(orderNumber) ? null : orderNumber.Trim();

    private static string? NormalizeShipmentNumber(string? shipmentNumber) =>
        string.IsNullOrWhiteSpace(shipmentNumber) ? null : shipmentNumber.Trim();

    private static DateTime? ToUtc(DateTime? value) => value.HasValue
        ? DateTime.SpecifyKind(value.Value.Date, DateTimeKind.Utc)
        : null;
}