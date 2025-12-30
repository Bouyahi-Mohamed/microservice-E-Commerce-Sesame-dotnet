using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        
        // Changing simple int Rate to detailed rating fields
        public double RatingStars { get; set; }
        public int RatingCount { get; set; }
        
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public int Stock { get; set; }
        public bool IsSolde { get; set; }
        
        // Storing keywords as comma-separated string or handling via DTO
        public string Keywords { get; set; }
    }
}
