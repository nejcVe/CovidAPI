using Microsoft.EntityFrameworkCore;
using CovidAPI.Models;
using CovidAPI.Services;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<CovidContext>(opt =>
    opt.UseInMemoryDatabase("CovidStats")); 
builder.Services.AddScoped<DataRetrievalService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var app = builder.Build();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetService<DataRetrievalService>();
if (service != null) {
    Console.WriteLine("Checking data...");
    await service.CheckData();
    Console.WriteLine("Loading data...");
    await service.LoadDataToMemory();
    Console.WriteLine("Finished");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
