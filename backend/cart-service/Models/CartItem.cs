using System.ComponentModel.DataAnnotations.Schema;

namespace cart_service.Models;

public class CartItem
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    
    // OrderId removed as this service manages Carts only.
    // If we migrate to Order, we move the item or create an OrderItem in OrderService.
    
    public int Quantity { get; set; }
    
    public int? DeliveryOptionId { get; set; }
    [ForeignKey("DeliveryOptionId")]
    public DeliveryOption? DeliveryOption { get; set; }
    
    public string? CartId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public decimal GetTotal()
    {
        return (Product?.Price ?? 0) * Quantity;
    }
}
