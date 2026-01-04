namespace cart_service.DTOs;

public class CartItemResponseDto
{
    public string _id { get; set; } = string.Empty;
    public ProductInCartDto product { get; set; } = new();
    public int quantity { get; set; }
    public DeliveryOptionDto? deliveryOption { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
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

public class AddToCartDto
{
    public string product { get; set; } = string.Empty; // Product ID
    public int quantity { get; set; }
}

public class UpdateCartDto
{
    public string id { get; set; } = string.Empty; // Cart item ID
    public int quantity { get; set; }
}

public class DeleteCartDto
{
    public string id { get; set; } = string.Empty; // Cart item ID
}

public class UpdateDeliveryDto
{
    public string id { get; set; } = string.Empty; // Cart item ID
    public string deliveryOption { get; set; } = string.Empty; // Delivery option ID
}
