using TargetApi.Models;
using TargetApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigurationSettings.Configuration = builder.Configuration;

builder.Services.AddScoped<IWidgetRepository, WidgetRepository>();
builder.Services.AddScoped<IPayloadRepository, PayloadRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
