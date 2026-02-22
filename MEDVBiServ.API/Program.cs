using MEDVBiServ.Application.Services;
using System.Text.Json.Serialization;
using MEDVBiServ.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS: Frontend-Origin erlauben
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:7090",
                "http://localhost:7090",
                "http://localhost:5127",   // ✅ DAS ist dein aktueller Origin
                "https://localhost:5127"   // optional
            )
            .AllowAnyHeader()
            .AllowAnyMethod();

        // .AllowCredentials(); // NUR wenn du Cookies/SignalR brauchst
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// In Dev kannst du https redirection ruhig anlassen
app.UseHttpsRedirection();


// ✅ richtige Policy
app.UseRouting();
app.UseCors("DevCors");
app.UseAuthorization();
app.MapControllers();



app.MapControllers();
app.MapGet("/ping", () => Results.Ok("API läuft!"));

app.Run();