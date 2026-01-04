namespace order_service.DTOs;

public class OrderResponseDto
{
    public string _id { get; set; } = string.Empty;
    public DateTime dateOrdered { get; set; }
    public bool complete { get; set; }
    public string transactionId { get; set; } = string.Empty;
    public List<OrderItemDto> orderItems { get; set; } = new();
    public decimal cartTotal { get; set; }
    public decimal shippingCost { get; set; }
    public decimal tax { get; set; }
    public decimal totalAmount { get; set; }
}

public class OrderItemDto
{
    public string _id { get; set; } = string.Empty;
    public ProductInCartDto product { get; set; } = new();
    public int quantity { get; set; }
    public DeliveryOptionDto? deliveryOption { get; set; }
}

public class ProductInCartDto
{
    public string _id { get; set; } = string.Empty;
    public string image { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public int priceCents { get; set; }
    public string description { get; set; } = string.Empty;
    public List<string> keywords { get; set; } = new();
    public RatingDto rating { get; set; } = new();
}

public class RatingDto
{
    public double stars { get; set; }
    public int count { get; set; }
}

public class DeliveryOptionDto
{
    public string _id { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public int priceCents { get; set; }
    public int estimatedDays { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}

public class PlaceOrderDto
{
    public int? customerId { get; set; }
    // Future: List<CartItemDto> items if pushing data instead of pulling
}
