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
builder.Services.AddLocalization(options => options.ResourcesPath = "Properties");

// Configure supported cultures
var supportedCultures = new[] { "en-US", "es-US" };
var defaultCulture = "en-US";

// Build the app
var host = builder.Build();


// Set the initial culture
var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
var preferredCulture = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "preferredCulture");

// Use the preferred culture if available, otherwise use the default
var culture = preferredCulture != null && supportedCultures.Contains(preferredCulture) 
    ? preferredCulture 
    : defaultCulture;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

await host.RunAsync();