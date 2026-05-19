using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Webshop.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient with BaseAddress pointing to the MVC server (where the API endpoints live)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7117/") });

await builder.Build().RunAsync();
