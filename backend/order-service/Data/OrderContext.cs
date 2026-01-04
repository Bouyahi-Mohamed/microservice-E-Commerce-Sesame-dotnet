using Microsoft.EntityFrameworkCore;
using order_service.Models;

namespace order_service.Data;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShippingAddress> ShippingAddresses { get; set; }
    public DbSet<Product> Products { get; set; } // Replica
    public DbSet<Category> Categories { get; set; } // Replica
    public DbSet<DeliveryOption> DeliveryOptions { get; set; } // Replica
}
