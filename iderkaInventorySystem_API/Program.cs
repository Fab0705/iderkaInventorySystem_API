using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Repository;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<iUser, UserRepository>();
builder.Services.AddScoped<iLogisticChief, LogisticChiefRepository>();
builder.Services.AddScoped<iStorageLocation, StorageLocationRepository>();
builder.Services.AddScoped<iRegion, RegionRepository>();
builder.Services.AddScoped<iSparePart, SparePartRepository>();
builder.Services.AddScoped<iTransfer, TransferRepository>();
builder.Services.AddScoped<iOrder, OrderRepository>();
builder.Services.AddScoped<iNotification, NotificationsRepository>();
builder.Services.AddScoped<iEmail, EmailRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build(); // Moved this line above app.UseCors to ensure 'app' is declared before usage

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
