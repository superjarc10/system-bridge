using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SystemBridge.Client;
using SystemBridge.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Uses the page's own host so the same build works from any device on the network.
var apiHost = new Uri(builder.HostEnvironment.BaseAddress).Host;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"http://{apiHost}:5005/") });
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<LocalizationService>();

await builder.Build().RunAsync();
