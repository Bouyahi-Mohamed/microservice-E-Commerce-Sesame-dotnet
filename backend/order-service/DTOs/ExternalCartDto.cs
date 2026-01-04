namespace order_service.DTOs;

public class ExternalCartItemDto
{
    public string _id { get; set; } = string.Empty;
    public ExternalProductInCartDto product { get; set; } = new();
    public int quantity { get; set; }
    public ExternalDeliveryOptionDto? deliveryOption { get; set; }
}

public class ExternalProductInCartDto
{
    public string _id { get; set; } = string.Empty;
    public string image { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public int priceCents { get; set; }
    public string description { get; set; } = string.Empty;
    public List<string> keywords { get; set; } = new();
    public ExternalRatingDto rating { get; set; } = new();
}

public class ExternalRatingDto
{
    public double stars { get; set; }
    public int count { get; set; }
}

public class ExternalDeliveryOptionDto
{
    public string _id { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public int priceCents { get; set; }
    public int estimatedDays { get; set; }
}
