namespace catalog_service.DTOs;

public class ProductResponseDto
{
    public string _id { get; set; } = string.Empty;
    public string image { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public RatingDto rating { get; set; } = new();
    public int priceCents { get; set; }
    public List<string> keywords { get; set; } = new();
    public string? type { get; set; }
    public string sizeChartLink { get; set; } = string.Empty;
}

public class RatingDto
{
    public double stars { get; set; }
    public int count { get; set; }
}
