using System;

namespace project_entities
{
    public class DeliveryOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceCents { get; set; }
        public int EstimatedDays { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
