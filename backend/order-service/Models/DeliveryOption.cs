namespace order_service.Models;

public class DeliveryOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PriceCents { get; set; }
    public int EstimatedDays { get; set; }
}
