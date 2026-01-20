using CleanCode.Infrastructure.Database;
using CleanCode.Infrastructure;
using CleanCodeLibrary.Domain.Persistance.Common;
using CleanCodeLibrary.Domain.Persistance.Students; 
using Microsoft.EntityFrameworkCore;
using CleanCode.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//jel potrebno i ovo i ono doli?
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CleanCode Library API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanCode Library v1");
        c.RoutePrefix = "swagger"; // možeš promijeniti u "" ako želiš da bude na rootu
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
