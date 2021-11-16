using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SolutionTemplate.BlazorUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var root_components = builder.RootComponents;
root_components.Add<App>("#app");
root_components.Add<HeadOutlet>("head::after");

var services = builder.Services;
services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();

