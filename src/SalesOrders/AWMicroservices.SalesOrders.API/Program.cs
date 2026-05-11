using AWMicroservices.SalesOrders.API.Extensions;
using Serilog;
using AWMicroservices.SalesOrders.Application;
using AWMicroservices.SalesOrders.Infrastructure;
using Serilog.Core;



var builder = WebApplication.CreateBuilder(args);

// Register Serilog logging as early as possible
builder.Host.AddSerilogLogging();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();

    Log.Information("Application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
    throw;
}
finally
{
    Log.CloseAndFlush();
}



