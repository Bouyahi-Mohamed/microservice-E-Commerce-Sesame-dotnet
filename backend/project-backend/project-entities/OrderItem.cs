using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_entities
{
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
        public string? CartId { get; set; } // Identifies which cart this belongs to (e.g. "guest-uuid" or "user-1")
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public decimal GetTotal()
        {
            return (Product?.Price ?? 0) * Quantity;
        }
    }
}
