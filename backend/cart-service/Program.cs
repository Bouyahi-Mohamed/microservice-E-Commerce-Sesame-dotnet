using Steeltoe.Discovery.Eureka;
using Steeltoe.Configuration.ConfigServer;
using Microsoft.EntityFrameworkCore;
using cart_service.Data;
using cart_service.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Config Server
builder.AddConfigServer();

// Add services to the container.
builder.Services.AddEurekaDiscoveryClient();

builder.Services.AddControllers();

builder.Services.AddDbContext<CartContext>(options =>
    options.UseInMemoryDatabase("CartDb"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CartContext>();
    context.Database.EnsureCreated();
    
    if (!context.Products.Any())
    {
        var category = new Category { Name = "Clothing" }; // Minimal replica
        context.Categories.Add(category);
        
        context.Products.AddRange(
            new Product 
            { 
                Name = "T-Shirt", 
                Description = "Cool T-Shirt", 
                Price = 19.99m, 
                Category = category,
                RatingStars = 4.5,
                RatingCount = 10,
                Keywords = "shirt,clothing",
                Image = "images/products/tshirt.png"
            },
            new Product 
            { 
                Name = "Jeans", 
                Description = "Blue Jeans", 
                Price = 49.99m, 
                Category = category,
                RatingStars = 4.8,
                RatingCount = 5,
                Keywords = "jeans,clothing",
                Image = "images/products/jeans.png"
            }
        );
        
        if (!context.DeliveryOptions.Any())
        {
            context.DeliveryOptions.Add(new DeliveryOption 
            { 
                Name = "Standard Delivery", 
                PriceCents = 499, 
                EstimatedDays = 3 
            });
        }
        
        context.SaveChanges();
    }
}

app.Run();
