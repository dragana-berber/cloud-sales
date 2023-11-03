global using CloudSalesAPI.Models;
using CloudSalesAPI.Data;
using CloudSalesAPI.Provider;
using CloudSalesAPI.Services.AdministrationService;
using CloudSalesAPI.Services.ProvisioningService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAdministrationService, AdministrationService>();
builder.Services.AddScoped<IProvisioningService, ProvisioningService>();
builder.Services.AddScoped<ICloudComputingProvider, MockedCloudComputingProvider>();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
