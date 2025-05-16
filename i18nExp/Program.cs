using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using i18nExp;
using System.Globalization;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure supported cultures
// var supportedCultures = new[] { "en-US", "es-MX" };
var defaultCulture = "es-MX";



// Build the app
var host = builder.Build();

// Set the initial culture
var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
var preferredCulture = await jsRuntime.InvokeAsync<string>("blazorCulture.get");

if (preferredCulture == null)
{
    await jsRuntime.InvokeVoidAsync("blazorCulture.set", defaultCulture);
}

// Use the preferred culture if available, otherwise use the default
var culture = preferredCulture ?? defaultCulture;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

await host.RunAsync();