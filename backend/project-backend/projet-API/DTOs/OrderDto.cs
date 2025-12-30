using System;
using System.Collections.Generic;

namespace projet_API.DTOs
{
    public class OrderResponseDto
    {
        public string _id { get; set; }
        public DateTime dateOrdered { get; set; }
        public bool complete { get; set; }
        public string transactionId { get; set; }
        public List<OrderItemDto> orderItems { get; set; }
        public decimal cartTotal { get; set; }
        public decimal shippingCost { get; set; }
        public decimal tax { get; set; }
        public decimal totalAmount { get; set; }
    }

    public class OrderItemDto
    {
        public string _id { get; set; }
        public ProductInCartDto product { get; set; }
        public int quantity { get; set; }
        public DeliveryOptionDto deliveryOption { get; set; }
    }

    public class PlaceOrderDto
    {
        // Can include customer info, shipping address, etc. if needed
        public int? customerId { get; set; }
    }
}
