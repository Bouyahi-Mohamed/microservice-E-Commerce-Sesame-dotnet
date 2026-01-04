using System.ComponentModel.DataAnnotations.Schema;

namespace order_service.Models;

public class Order
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    // Customer navigation removed or we can keep it if we replicate Customer.
    // keeping it simple for now:
    
    public DateTime DateOrdered { get; set; } = DateTime.Now;
    public bool Complete { get; set; }
    public string? TransactionId { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
