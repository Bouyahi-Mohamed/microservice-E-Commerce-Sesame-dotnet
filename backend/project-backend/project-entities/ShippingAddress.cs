using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_entities
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        
        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
