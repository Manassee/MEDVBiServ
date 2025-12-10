using MEDVBiServ.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ConnectionStrings
var deCon = builder.Configuration.GetConnectionString("DE_Bible");
var frCon = builder.Configuration.GetConnectionString("FR_Bible");
var enCon = builder.Configuration.GetConnectionString("EN_Bible");

// 1️⃣ DE-Datenbank
builder.Services.AddDbContext<DEDbContext>(options =>
    options.UseSqlite(deCon));

// 2️⃣ FR-Datenbank
builder.Services.AddDbContext<FRDbContext>(options =>
    options.UseSqlite(frCon));

// 3️⃣ EN-Datenbank
builder.Services.AddDbContext<ENDbContext>(options =>
    options.UseSqlite(enCon));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWasm",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWasm");

app.MapControllers();
app.Run();
