using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace project_entities
{
    public class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public bool Complete { get; set; }
        public string? TransactionId { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }

        public decimal GetCartTotal()
        {
            return OrderItems?.Sum(item => item.GetTotal()) ?? 0;
        }

        public int GetCartItems()
        {
             return OrderItems?.Sum(item => item.Quantity) ?? 0;
        }
        
        // Shipping cost hardcoded as per Django logic
        public decimal GetShippingCost()
        {
            return 6.99m;
        }

        public decimal GetCartTotalPlusShipping()
        {
            return GetCartTotal() + GetShippingCost();
        }

        public decimal GetEstimatedTax()
        {
            return GetCartTotalPlusShipping() * 0.1m;
        }

        public decimal GetCartTotalPlusShippingAndTax()
        {
             return GetCartTotalPlusShipping() + GetEstimatedTax();
        }
    }
}
