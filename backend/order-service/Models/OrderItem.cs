using System.ComponentModel.DataAnnotations.Schema;

namespace order_service.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    
    public int? OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order? Order { get; set; }
    
    public int Quantity { get; set; }
    
    public int? DeliveryOptionId { get; set; }
    [ForeignKey("DeliveryOptionId")]
    public DeliveryOption? DeliveryOption { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
