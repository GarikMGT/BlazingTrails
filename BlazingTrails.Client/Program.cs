using BlazingTrails.Client;
using BlazingTrails.Client.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddTrailHandler).Assembly));

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddOidcAuthentication();
await builder.Build().RunAsync();