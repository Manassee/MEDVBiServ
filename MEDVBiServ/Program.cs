using MEDVBiServ.Clients;
using MEDVBiServ.Interfaces;
using MEDVBiServ.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

using static MEDVBiServ.Pages.CreatePage;

namespace MEDVBiServ
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddSingleton<SlideService>();


            // MudBlazor
            builder.Services.AddMudServices();

            // HttpClient (ruft Deine API auf)
            builder.Services.AddScoped(sp => new HttpClient
            {
                //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                BaseAddress = new Uri("http://localhost:5294/") // API-Base-URL
            });


            // ✅ UI-Client registrieren
            builder.Services.AddScoped<IBibleApiClient, BibleApiClient>();


            await builder.Build().RunAsync();
        }
    }
}
