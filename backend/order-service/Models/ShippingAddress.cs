using System.ComponentModel.DataAnnotations.Schema;

namespace order_service.Models;

public class ShippingAddress
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order? Order { get; set; }
    
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zipcode { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; } = DateTime.Now;
}
