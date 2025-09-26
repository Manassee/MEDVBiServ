using MEDVBiServ.App.Application.Interfaces;
using MEDVBiServ.App.Application.Services;
using MEDVBiServ.App.Infrastructure.DbContext;
using MEDVBiServ.App.Infrastructure.DbFactories;
using MEDVBiServ.App.Infrastructure.Repository;
using MEDVBiServ.App.Infrastructure.Repository.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Endpunkte/Frameworks
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // nur falls du Controller nutzt

// SQLite: 3 Kontexte registrieren (Factories + DbContexts + HealthChecks)
builder.Services.AddSqliteDbContexts<DEDbContext, FRDbContext, ENDbContext>(builder.Configuration);

// Repositories registrieren – nutze die Wrapper-Klassen:
builder.Services.AddScoped<IDeBibleRepository, DeBibleRepository>();
builder.Services.AddScoped<IFrBibleRepository, FrBibleRepository>();



// Router + LanguageProvider
builder.Services.AddScoped<IBibleRepositoryRouter, BibleRepositoryRouter>();
builder.Services.AddScoped<ILanguageProvider, LanguageProvider>();


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
// --- Smoke-Tests ---

// 0) Sanity-Check
app.MapGet("/ping", () => Results.Text("pong"));

// 1) Deutsch: /test/de/{book}/{chapter}
app.MapGet("/test/de/{book:int}/{chapter:int}", async (
    int book,
    int chapter,
    MEDVBiServ.App.Infrastructure.Repository.interfaces.IDeBibleRepository repo,
    CancellationToken ct) =>
{
    var verses = await repo.GetChapterAsync(book, chapter, ct);
    return Results.Ok(verses);
});

// 2) Französisch: /test/fr/{book}/{chapter}
app.MapGet("/test/fr/{book:int}/{chapter:int}", async (
    int book,
    int chapter,
    MEDVBiServ.App.Infrastructure.Repository.interfaces.IFrBibleRepository repo,
    CancellationToken ct) =>
{
    var verses = await repo.GetChapterAsync(book, chapter, ct);
    return Results.Ok(verses);
});
await app.RunAsync();
