using iderkaInventorySystem_API.Repository;
using iderkaInventorySystem_API.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Add(new ServiceDescriptor(typeof(iUser), new UserRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iLogisticChief), new LogisticChiefRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iStorageLocation), new StorageLocationRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iRegion), new RegionRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iSparePart), new SparePartRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iTransfer), new TransferRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iOrder), new OrderRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iNotification), new NotificationsRepository()));
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
