using ChartAPI.Hubs;
using ChartAPI.Models;
using ChartAPI.Subscription;
using ChartAPI.Subscription.Middleware;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DatabaseSubscription<Sale>>();
builder.Services.AddSingleton<DatabaseSubscription<Staff>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseDatabaseSubscription<DatabaseSubscription<Sale>>("Sales");
app.UseDatabaseSubscription<DatabaseSubscription<Staff>>("Staff");

// CORS politikasýný kullan
app.UseCors("AllowSpecificOrigins");

// SignalR hub'ýný tanýmlayýn
app.MapHub<SalesHub>("/saleshub");

app.MapGet("/", () => "Hello World!");
app.Run();
