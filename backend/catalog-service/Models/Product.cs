using System.ComponentModel.DataAnnotations.Schema;

namespace catalog_service.Models;

public class Product
{
    public int Id { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public double RatingStars { get; set; }
    public int RatingCount { get; set; }
    
    public decimal Price { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    public int Stock { get; set; }
    public bool IsSolde { get; set; }
    
    public string Keywords { get; set; } = string.Empty;
}
