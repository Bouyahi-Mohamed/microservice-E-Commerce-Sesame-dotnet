using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_context;
using project_entities;
using projet_API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace projet_API.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;

        public OrdersController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto>> PlaceOrder([FromBody] PlaceOrderDto request)
        {
            // Get customerId from JWT token
            var customerIdClaim = User.FindFirst("customerId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
            {
                return Unauthorized(new { message = "Invalid authentication token" });
            }

            // Get all cart items (OrderItems where OrderId is null AND CartId is "user-{id}")
            string userCartId = $"user-{User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value}";
            
            var cartItems = await _context.OrderItems
                .Where(oi => oi.OrderId == null && oi.CartId == userCartId)
                .Include(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                .Include(oi => oi.DeliveryOption)
                .ToListAsync();

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
                TransactionId = Guid.NewGuid().ToString() // Generate unique transaction ID
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Assign OrderId to all cart items (moves them from cart to order)
            foreach (var item in cartItems)
            {
                item.OrderId = order.Id;
                item.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            // Calculate totals using delivery option prices
            var cartTotal = cartItems.Sum(item => item.Product.Price * item.Quantity);
            var shippingCost = cartItems.Sum(item => (item.DeliveryOption?.PriceCents ?? 0) / 100m);
            var subtotal = cartTotal + shippingCost;
            var tax = subtotal * 0.1m;
            var totalAmount = subtotal + tax;

            // Map to DTO
            var orderDto = new OrderResponseDto
            {
                _id = order.Id.ToString(),
                dateOrdered = order.DateOrdered,
                complete = order.Complete,
                transactionId = order.TransactionId,
                orderItems = cartItems.Select(item => new OrderItemDto
                {
                    _id = item.Id.ToString(),
                    product = new ProductInCartDto
                    {
                        _id = item.Product.Id.ToString(),
                        image = item.Product.Image,
                        name = item.Product.Name,
                        priceCents = (int)(item.Product.Price * 100),
                        description = item.Product.Description,
                        keywords = string.IsNullOrEmpty(item.Product.Keywords)
                            ? new List<string>()
                            : item.Product.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                        rating = new RatingDto
                        {
                            stars = item.Product.RatingStars,
                            count = item.Product.RatingCount
                        }
                    },
                    quantity = item.Quantity,
                    deliveryOption = item.DeliveryOption != null ? new DeliveryOptionDto
                    {
                        _id = item.DeliveryOption.Id.ToString(),
                        name = item.DeliveryOption.Name,
                        priceCents = item.DeliveryOption.PriceCents,
                        estimatedDays = item.DeliveryOption.EstimatedDays,
                        createdAt = item.DeliveryOption.CreatedAt,
                        updatedAt = item.DeliveryOption.UpdatedAt
                    } : null
                }).ToList(),
                cartTotal = cartTotal,
                shippingCost = shippingCost,
                tax = tax,
                totalAmount = totalAmount
            };

            return Ok(orderDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Category)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.DeliveryOption)
                .ToListAsync();

            var orderDtos = orders.Select(order =>
            {
                var cartTotal = order.OrderItems.Sum(item => item.Product.Price * item.Quantity);
                var shippingCost = order.OrderItems.Sum(item => (item.DeliveryOption?.PriceCents ?? 0) / 100m);
                var subtotal = cartTotal + shippingCost;
                var tax = subtotal * 0.1m;

                return new OrderResponseDto
                {
                    _id = order.Id.ToString(),
                    dateOrdered = order.DateOrdered,
                    complete = order.Complete,
                    transactionId = order.TransactionId,
                    orderItems = order.OrderItems.Select(item => new OrderItemDto
                    {
                        _id = item.Id.ToString(),
                        product = new ProductInCartDto
                        {
                            _id = item.Product.Id.ToString(),
                            image = item.Product.Image,
                            name = item.Product.Name,
                            priceCents = (int)(item.Product.Price * 100),
                            description = item.Product.Description,
                            keywords = string.IsNullOrEmpty(item.Product.Keywords)
                                ? new List<string>()
                                : item.Product.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                            rating = new RatingDto
                            {
                                stars = item.Product.RatingStars,
                                count = item.Product.RatingCount
                            }
                        },
                        quantity = item.Quantity,
                        deliveryOption = item.DeliveryOption != null ? new DeliveryOptionDto
                        {
                            _id = item.DeliveryOption.Id.ToString(),
                            name = item.DeliveryOption.Name,
                            priceCents = item.DeliveryOption.PriceCents,
                            estimatedDays = item.DeliveryOption.EstimatedDays,
                            createdAt = item.DeliveryOption.CreatedAt,
                            updatedAt = item.DeliveryOption.UpdatedAt
                        } : null
                    }).ToList(),
                    cartTotal = cartTotal,
                    shippingCost = shippingCost,
                    tax = tax,
                    totalAmount = subtotal + tax
                };
            }).ToList();

            return Ok(orderDtos);
        }
    }
}
