using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SystemBridge.Client.Models;

namespace SystemBridge.Client.Services;

public sealed class ApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private string? token;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(token);
    public string UserRole { get; private set; } = string.Empty;
    public string CurrentUsername { get; private set; } = string.Empty;

    public bool IsAdmin => IsAuthenticated && (string.Equals(UserRole, "Admin", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(UserRole));
    public bool IsWorker => IsAuthenticated && string.Equals(UserRole, "Worker", StringComparison.OrdinalIgnoreCase);

    public event Action? OnAuthStateChanged;

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        using var response = await httpClient.PostAsJsonAsync("api/auth/login", request, JsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        token = result?.Token;
        UserRole = result?.Role ?? string.Empty;
        CurrentUsername = result?.Username ?? request.Username;
        OnAuthStateChanged?.Invoke();
        return result;
    }

    public void Logout()
    {
        token = null;
        UserRole = string.Empty;
        CurrentUsername = string.Empty;
        OnAuthStateChanged?.Invoke();
    }

    public Task<Customer?> AddCustomerAsync(CustomerRequest customer) => SendJsonAsync<CustomerRequest, Customer>(HttpMethod.Post, "api/customers", customer);
    public Task<Customer?> UpdateCustomerAsync(int id, CustomerRequest customer) => SendJsonAsync<CustomerRequest, Customer>(HttpMethod.Put, $"api/customers/{id}", customer);
    public Task<bool> RemoveCustomerAsync(int id) => SendDeleteAsync($"api/customers/{id}");
    public Task<List<Customer>> GetCustomersAsync() => GetManyAsync<Customer>("api/customers");
    public Task<Customer?> GetCustomerAsync(int id) => GetOneAsync<Customer>($"api/customers/{id}");

    public Task<Product?> AddProductAsync(ProductRequest product) => SendJsonAsync<ProductRequest, Product>(HttpMethod.Post, "api/products", product);
    public Task<Product?> UpdateProductAsync(int id, ProductRequest product) => SendJsonAsync<ProductRequest, Product>(HttpMethod.Put, $"api/products/{id}", product);
    public Task<bool> RemoveProductAsync(int id) => SendDeleteAsync($"api/products/{id}");
    public Task<List<Product>> GetProductsAsync() => GetManyAsync<Product>("api/products");
    public Task<Product?> GetProductAsync(int id) => GetOneAsync<Product>($"api/products/{id}");

    public Task<ProductPackaging?> AddProductPackagingAsync(ProductPackagingRequest packaging) => SendJsonAsync<ProductPackagingRequest, ProductPackaging>(HttpMethod.Post, "api/product-packaging", packaging);
    public Task<ProductPackaging?> UpdateProductPackagingAsync(int id, ProductPackagingRequest packaging) => SendJsonAsync<ProductPackagingRequest, ProductPackaging>(HttpMethod.Put, $"api/product-packaging/{id}", packaging);
    public Task<bool> RemoveProductPackagingAsync(int id) => SendDeleteAsync($"api/product-packaging/{id}");
    public Task<List<ProductPackaging>> GetProductPackagingAsync() => GetManyAsync<ProductPackaging>("api/product-packaging");
    public Task<ProductPackaging?> GetProductPackagingAsync(int id) => GetOneAsync<ProductPackaging>($"api/product-packaging/{id}");

    public Task<PackagingType?> AddPackagingTypeAsync(PackagingTypeRequest type) => SendJsonAsync<PackagingTypeRequest, PackagingType>(HttpMethod.Post, "api/packaging-types", type);
    public Task<PackagingType?> UpdatePackagingTypeAsync(int id, PackagingTypeRequest type) => SendJsonAsync<PackagingTypeRequest, PackagingType>(HttpMethod.Put, $"api/packaging-types/{id}", type);
    public Task<bool> RemovePackagingTypeAsync(int id) => SendDeleteAsync($"api/packaging-types/{id}");
    public Task<List<PackagingType>> GetPackagingTypesAsync() => GetManyAsync<PackagingType>("api/packaging-types");
    public Task<PackagingType?> GetPackagingTypeAsync(int id) => GetOneAsync<PackagingType>($"api/packaging-types/{id}");

    public Task<Pallet?> AddPalletAsync(PalletRequest pallet) => SendJsonAsync<PalletRequest, Pallet>(HttpMethod.Post, "api/pallets", pallet);
    public Task<Pallet?> UpdatePalletAsync(int id, PalletRequest pallet) => SendJsonAsync<PalletRequest, Pallet>(HttpMethod.Put, $"api/pallets/{id}", pallet);
    public Task<bool> RemovePalletAsync(int id) => SendDeleteAsync($"api/pallets/{id}");
    public Task<List<Pallet>> GetPalletsAsync() => GetManyAsync<Pallet>("api/pallets");
    public Task<Pallet?> GetPalletAsync(int id) => GetOneAsync<Pallet>($"api/pallets/{id}");

    public Task<Order?> AddOrderAsync(OrderRequest order) => SendJsonAsync<OrderRequest, Order>(HttpMethod.Post, "api/orders", order);
    public Task<Order?> UpdateOrderAsync(int id, OrderRequest order) => SendJsonAsync<OrderRequest, Order>(HttpMethod.Put, $"api/orders/{id}", order);
    public Task<bool> RemoveOrderAsync(int id) => SendDeleteAsync($"api/orders/{id}");
    public Task<List<Order>> GetOrdersAsync() => GetManyAsync<Order>("api/orders");
    public Task<Order?> GetOrderAsync(int id) => GetOneAsync<Order>($"api/orders/{id}");
    public Task<List<ChangeHistory>> GetOrderHistoryAsync(int id) => GetManyAsync<ChangeHistory>($"api/orders/{id}/history");
    public Task<bool> ClearOrderHistoryAsync(int id) => SendDeleteAsync($"api/orders/{id}/history");

    public Task<OrderItem?> AddOrderItemAsync(OrderItemRequest item) => SendJsonAsync<OrderItemRequest, OrderItem>(HttpMethod.Post, "api/order-items", item);
    public Task<OrderItem?> UpdateOrderItemAsync(int id, OrderItemRequest item) => SendJsonAsync<OrderItemRequest, OrderItem>(HttpMethod.Put, $"api/order-items/{id}", item);
    public Task<bool> RemoveOrderItemAsync(int id) => SendDeleteAsync($"api/order-items/{id}");
    public Task<List<OrderItem>> GetOrderItemsAsync() => GetManyAsync<OrderItem>("api/order-items");
    public Task<OrderItem?> GetOrderItemAsync(int id) => GetOneAsync<OrderItem>($"api/order-items/{id}");

    public Task<Shipment?> AddShipmentAsync(ShipmentRequest shipment) => SendJsonAsync<ShipmentRequest, Shipment>(HttpMethod.Post, "api/shipments", shipment);
    public Task<Shipment?> UpdateShipmentAsync(int id, ShipmentRequest shipment) => SendJsonAsync<ShipmentRequest, Shipment>(HttpMethod.Put, $"api/shipments/{id}", shipment);
    public Task<bool> RemoveShipmentAsync(int id) => SendDeleteAsync($"api/shipments/{id}");
    public Task<List<Shipment>> GetShipmentsAsync() => GetManyAsync<Shipment>("api/shipments");
    public Task<Shipment?> GetShipmentAsync(int id) => GetOneAsync<Shipment>($"api/shipments/{id}");
    public Task<List<ChangeHistory>> GetShipmentHistoryAsync(int id) => GetManyAsync<ChangeHistory>($"api/shipments/{id}/history");
    public Task<bool> ClearShipmentHistoryAsync(int id) => SendDeleteAsync($"api/shipments/{id}/history");

    public Task<ShipmentItem?> AddShipmentItemAsync(ShipmentItemRequest item) => SendJsonAsync<ShipmentItemRequest, ShipmentItem>(HttpMethod.Post, "api/shipment-items", item);
    public Task<ShipmentItem?> UpdateShipmentItemAsync(int id, ShipmentItemRequest item) => SendJsonAsync<ShipmentItemRequest, ShipmentItem>(HttpMethod.Put, $"api/shipment-items/{id}", item);
    public Task<bool> RemoveShipmentItemAsync(int id) => SendDeleteAsync($"api/shipment-items/{id}");
    public Task<List<ShipmentItem>> GetShipmentItemsAsync() => GetManyAsync<ShipmentItem>("api/shipment-items");
    public Task<ShipmentItem?> GetShipmentItemAsync(int id) => GetOneAsync<ShipmentItem>($"api/shipment-items/{id}");

    public Task<InventoryItem?> AddInventoryItemAsync(InventoryItemRequest item) => SendJsonAsync<InventoryItemRequest, InventoryItem>(HttpMethod.Post, "api/inventory-items", item);
    public Task<InventoryItem?> UpdateInventoryItemAsync(int id, InventoryItemRequest item) => SendJsonAsync<InventoryItemRequest, InventoryItem>(HttpMethod.Put, $"api/inventory-items/{id}", item);
    public Task<InventoryItem?> ConfirmInventoryItemAsync(int id) => SendJsonAsync<object?, InventoryItem>(HttpMethod.Post, $"api/inventory-items/{id}/confirm", null);
    public Task<bool> RemoveInventoryItemAsync(int id) => SendDeleteAsync($"api/inventory-items/{id}");
    public Task<List<InventoryItem>> GetInventoryItemsAsync() => GetManyAsync<InventoryItem>("api/inventory-items");
    public Task<InventoryItem?> GetInventoryItemAsync(int id) => GetOneAsync<InventoryItem>($"api/inventory-items/{id}");

    private HttpRequestMessage CreateRequest(HttpMethod method, string uri)
    {
        var request = new HttpRequestMessage(method, uri);
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return request;
    }

    private async Task<T?> SendAsync<T>(HttpRequestMessage request)
    {
        using var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    private async Task<TResponse?> SendJsonAsync<TRequest, TResponse>(HttpMethod method, string uri, TRequest value)
    {
        using var request = CreateRequest(method, uri);
        request.Content = JsonContent.Create(value, options: JsonOptions);
        return await SendAsync<TResponse>(request);
    }

    private async Task<bool> SendDeleteAsync(string uri)
    {
        using var request = CreateRequest(HttpMethod.Delete, uri);
        using var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    private async Task<List<T>> GetManyAsync<T>(string uri)
    {
        using var request = CreateRequest(HttpMethod.Get, uri);
        return await SendAsync<List<T>>(request) ?? [];
    }

    private Task<T?> GetOneAsync<T>(string uri)
    {
        return SendAsync<T>(CreateRequest(HttpMethod.Get, uri));
    }
}
