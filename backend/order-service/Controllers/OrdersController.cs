using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using order_service.Data;
using order_service.Models;
using order_service.DTOs;
using order_service.Services;
using System.Security.Claims;

namespace order_service.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderContext _context;
    private readonly ICartService _cartService;

    public OrdersController(OrderContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<OrderResponseDto>> PlaceOrder([FromBody] PlaceOrderDto request)
    {
        // Get customerId from JWT token
        var customerIdClaim = User.FindFirst("customerId")?.Value;
        if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
        {
            return Unauthorized(new { message = "Invalid authentication token: missing customerId" });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // Get authentication token to forward
        string? token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // Fetch cart items from Cart Service
        var cartItems = await _cartService.GetCartAsync(userId, token);

        if (!cartItems.Any())
        {
            return BadRequest("Cart is empty");
        }

        // Create new order
        var order = new Order
        {
            CustomerId = customerId,
            DateOrdered = DateTime.Now,
            Complete = false,
            TransactionId = Guid.NewGuid().ToString()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); // detailed save to get ID

        var orderItemsList = new List<OrderItem>();

        foreach (var item in cartItems)
        {
            if (!int.TryParse(item.product._id, out int productId)) continue;

            // Ensure product exists (Snapshot pattern)
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                product = new Product
                {
                    Id = productId,
                    Name = item.product.name,
                    Description = item.product.description,
                    Price = item.product.priceCents / 100m,
                    Image = item.product.image,
                    RatingStars = item.product.rating.stars,
                    RatingCount = item.product.rating.count,
                    Keywords = string.Join(",", item.product.keywords),
                    CategoryId = 1 // Default or fetch category
                };
                
                // Ensure category exists
                if (!await _context.Categories.AnyAsync())
                {
                    _context.Categories.Add(new Category { Id = 1, Name = "Uncategorized" });
                }

                _context.Products.Add(product);
            }
            
            // Ensure delivery option exists
            int? deliveryOptionId = null;
            if (item.deliveryOption != null && int.TryParse(item.deliveryOption._id, out int dId))
            {
                var delivery = await _context.DeliveryOptions.FindAsync(dId);
                if (delivery == null)
                {
                    delivery = new DeliveryOption
                    {
                        Id = dId,
                        Name = item.deliveryOption.name,
                        PriceCents = item.deliveryOption.priceCents,
                        EstimatedDays = item.deliveryOption.estimatedDays
                    };
                    _context.DeliveryOptions.Add(delivery);
                }
                deliveryOptionId = dId;
            }

            var orderItem = new OrderItem
            {
                ProductId = productId,
                Quantity = item.quantity,
                DeliveryOptionId = deliveryOptionId,
                OrderId = order.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            orderItemsList.Add(orderItem);
            _context.OrderItems.Add(orderItem);
        }

        await _context.SaveChangesAsync();

        // Calculate totals
        var cartTotal = orderItemsList.Sum(item => (item.Product?.Price ?? 0) * item.Quantity);
        var shippingCost = orderItemsList.Sum(item => (item.DeliveryOption?.PriceCents ?? 0) / 100m);
        var subtotal = cartTotal + shippingCost;
        var tax = subtotal * 0.1m;
        var totalAmount = subtotal + tax;

        // Clear the cart
        await _cartService.ClearCartAsync(userId, token);

        return Ok(new OrderResponseDto
        {
            _id = order.Id.ToString(),
            dateOrdered = order.DateOrdered,
            complete = order.Complete,
            transactionId = order.TransactionId,
            orderItems = orderItemsList.Select(MapToOrderItemDto).ToList(),
            cartTotal = cartTotal,
            shippingCost = shippingCost,
            tax = tax,
            totalAmount = totalAmount
        });
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
    {
        // Filter by user? Monolith didn't filter by user in GetOrders (admin?) but usually users see their own.
        // We'll filter by CustomerId.
        
        var customerIdClaim = User.FindFirst("customerId")?.Value;
        if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
        {
             // If admin, maybe return all? For now, return empty or unauthorized.
             return Unauthorized();
        }

        var orders = await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.DeliveryOption)
            .ToListAsync();

        var orderDtos = orders.Select(order =>
        {
            var cartTotal = order.OrderItems.Sum(item => (item.Product?.Price ?? 0) * item.Quantity);
            var shippingCost = order.OrderItems.Sum(item => (item.DeliveryOption == null ? 0 : item.DeliveryOption.PriceCents) / 100m);
            var subtotal = cartTotal + shippingCost;
            var tax = subtotal * 0.1m;

            return new OrderResponseDto
            {
                _id = order.Id.ToString(),
                dateOrdered = order.DateOrdered,
                complete = order.Complete,
                transactionId = order.TransactionId ?? "",
                orderItems = order.OrderItems.Select(MapToOrderItemDto).ToList(),
                cartTotal = cartTotal,
                shippingCost = shippingCost,
                tax = tax,
                totalAmount = subtotal + tax
            };
        }).ToList();

        return Ok(orderDtos);
    }

    private OrderItemDto MapToOrderItemDto(OrderItem item)
    {
         return new OrderItemDto
         {
             _id = item.Id.ToString(),
             product = new ProductInCartDto
             {
                 _id = item.Product?.Id.ToString() ?? "",
                 image = item.Product?.Image ?? "",
                 name = item.Product?.Name ?? "",
                 priceCents = (int)((item.Product?.Price ?? 0) * 100),
                 description = item.Product?.Description ?? "",
                 rating = new RatingDto
                 {
                     stars = item.Product?.RatingStars ?? 0,
                     count = item.Product?.RatingCount ?? 0
                 },
                 keywords = string.IsNullOrEmpty(item.Product?.Keywords) ? new List<string>() : item.Product.Keywords.Split(',').ToList()
             },
             quantity = item.Quantity,
             deliveryOption = item.DeliveryOption != null ? new DeliveryOptionDto
             {
                 _id = item.DeliveryOption.Id.ToString(),
                 name = item.DeliveryOption.Name,
                 priceCents = item.DeliveryOption.PriceCents,
                 estimatedDays = item.DeliveryOption.EstimatedDays,
                 createdAt = DateTime.Now,
                 updatedAt = DateTime.Now
             } : null
         };
    }
}
