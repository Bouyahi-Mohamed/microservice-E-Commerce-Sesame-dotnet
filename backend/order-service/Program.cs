using Steeltoe.Discovery.Eureka;
using Steeltoe.Configuration.ConfigServer;
using Microsoft.EntityFrameworkCore;
using order_service.Data;
using order_service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Config Server
builder.AddConfigServer();

// Add services to the container.
builder.Services.AddEurekaDiscoveryClient();

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseInMemoryDatabase("OrderDb"));

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "YourApp",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "YourAppUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!"))
    };
});

// Http Client for Cart Service (Docker DNS based)
builder.Services.AddHttpClient<ICartService, CartService>(client =>
{
    // If using Eureka enabled HttpClient, we would use http://CART-SERVICE/
    // Since we are using basic HttpClient + Docker DNS
    client.BaseAddress = new Uri("http://cart-service:8080"); 
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
