using System;
using System.Collections.Generic;

namespace projet_API.DTOs
{
    public class CartItemResponseDto
    {
        public string _id { get; set; }
        public ProductInCartDto product { get; set; }
        public int quantity { get; set; }
        public DeliveryOptionDto deliveryOption { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class ProductInCartDto
    {
        public string _id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public int priceCents { get; set; }
        public string description { get; set; }
        public List<string> keywords { get; set; }
        public RatingDto rating { get; set; }
    }

    public class DeliveryOptionDto
    {
        public string _id { get; set; }
        public string name { get; set; }
        public int priceCents { get; set; }
        public int estimatedDays { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class AddToCartDto
    {
        public string product { get; set; }  // Product ID
        public int quantity { get; set; }
    }

    public class UpdateCartDto
    {
        public string id { get; set; }  // Cart item ID
        public int quantity { get; set; }
    }

    public class DeleteCartDto
    {
        public string id { get; set; }  // Cart item ID
    }

    public class UpdateDeliveryDto
    {
        public string id { get; set; }  // Cart item ID
        public string deliveryOption { get; set; }  // Delivery option ID
    }
}
