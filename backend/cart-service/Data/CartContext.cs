using Microsoft.EntityFrameworkCore;
using cart_service.Models;

namespace cart_service.Data;

public class CartContext : DbContext
{
    public CartContext(DbContextOptions<CartContext> options) : base(options)
    {
    }

    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Product> Products { get; set; } // Replica
    public DbSet<Category> Categories { get; set; } // Replica
    public DbSet<DeliveryOption> DeliveryOptions { get; set; } // Replica
}
