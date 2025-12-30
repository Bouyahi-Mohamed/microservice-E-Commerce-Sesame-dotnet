using project_context;
using project_entities;
using System.Linq;

namespace projet_API.Data
{
    public class DeliveryOptionSeeder
    {
        public static void Seed(DataContext context)
        {
            if (!context.DeliveryOptions.Any())
            {
                var options = new[]
                {
                    new DeliveryOption
                    {
                        Name = "Standard Delivery",
                        PriceCents = 599,
                        EstimatedDays = 6
                    },
                    new DeliveryOption
                    {
                        Name = "Express Delivery",
                        PriceCents = 1299,
                        EstimatedDays = 3
                    },
                    new DeliveryOption
                    {
                        Name = "Free Delivery",
                        PriceCents = 0,
                        EstimatedDays = 10
                    }
                };

                context.DeliveryOptions.AddRange(options);
                context.SaveChanges();
            }
        }
    }
}
