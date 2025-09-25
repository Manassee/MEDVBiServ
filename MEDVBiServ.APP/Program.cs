using MEDVBiServ.App.Infrastructure.DbContext;
using MEDVBiServ.App.Infrastructure.DbFactories; 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Endpunkte/Frameworks
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // nur falls du Controller nutzt

// SQLite: 3 Kontexte registrieren (Factories + DbContexts + HealthChecks)
builder.Services.AddSqliteDbContexts<DEDbContext, FRDbContext, ENDbContext>(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // falls Controller genutzt
app.MapHealthChecks("/health");

await app.RunAsync();
