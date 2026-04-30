using backend_vex.Configuration;
using Microsoft.EntityFrameworkCore;
using POCArgos.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddRepositories();
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("OrderStatuses", builder =>
        builder.Expire(TimeSpan.FromHours(3))
               .Tag("OrderStatusesTag")
               .SetVaryByRouteValue(["*"]));
    options.AddPolicy("ShippingMethods", builder =>
        builder.Expire(TimeSpan.FromHours(3))
               .Tag("ShippingMethodsTag")
               .SetVaryByRouteValue(["*"]));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:55353").WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// In Docker/Production behind a proxy, HTTPS is usually handled by the proxy (Nginx)
// or not needed for internal traffic.
// app.UseHttpsRedirection(); 

app.UseCors("AllowAngularApp");

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
