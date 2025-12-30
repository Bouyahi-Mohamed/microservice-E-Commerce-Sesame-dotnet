using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_context;
using project_entities;
using projet_API.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projet_API.Controllers
{
    [ApiController]
    [Route("cart")]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;

        public CartController(DataContext context)
        {
            _context = context;
        }

        private string GetCartId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                return $"user-{userId}";
            }

            // Check header
            if (Request.Headers.TryGetValue("Cart-ID", out var headerCartId))
            {
                return headerCartId.ToString();
            }

            return "anonymous";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemResponseDto>>> GetCart()
        {
            var cartId = GetCartId();

            // Cart items are OrderItems where OrderId is null AND CartId matches
            var cartItems = await _context.OrderItems
                .Where(oi => oi.OrderId == null && oi.CartId == cartId)
                .Include(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                .Include(oi => oi.DeliveryOption)
                .ToListAsync();

            var cartDtos = cartItems.Select(MapToCartItemDto).ToList();
            return Ok(cartDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemResponseDto>> AddToCart([FromBody] AddToCartDto request)
        {
            var cartId = GetCartId();

            // Parse product ID
            if (!int.TryParse(request.product, out int productId))
            {
                return BadRequest("Invalid product ID");
            }

            // Check if product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Get default delivery option (Standard)
            var defaultDelivery = await _context.DeliveryOptions
                .FirstOrDefaultAsync(d => d.Name == "Standard Delivery");

            // Check if item already in cart
            var existingItem = await _context.OrderItems
                .Include(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                .Include(oi => oi.DeliveryOption)
                .FirstOrDefaultAsync(oi => oi.ProductId == productId && oi.OrderId == null && oi.CartId == cartId);

            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += request.quantity;
                existingItem.UpdatedAt = System.DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(MapToCartItemDto(existingItem));
            }

            // Create new cart item
            var cartItem = new OrderItem
            {
                ProductId = productId,
                Quantity = request.quantity,
                DeliveryOptionId = defaultDelivery?.Id,
                OrderId = null, // Null means it's in cart
                CartId = cartId
            };

            _context.OrderItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Reload with includes
            var addedItem = await _context.OrderItems
                .Include(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                .Include(oi => oi.DeliveryOption)
                .FirstAsync(oi => oi.Id == cartItem.Id);

            return Ok(MapToCartItemDto(addedItem));
        }

        [HttpPatch]
        public async Task<ActionResult<CartItemResponseDto>> UpdateCartQuantity([FromBody] UpdateCartDto request)
        {
            var cartId = GetCartId();

            if (!int.TryParse(request.id, out int cartItemId))
            {
                return BadRequest("Invalid cart item ID");
            }

            var cartItem = await _context.OrderItems
                .Include(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                .Include(oi => oi.DeliveryOption)
                .FirstOrDefaultAsync(oi => oi.Id == cartItemId && oi.OrderId == null && oi.CartId == cartId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }

            cartItem.Quantity = request.quantity;
            cartItem.UpdatedAt = System.DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(MapToCartItemDto(cartItem));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCartItem([FromBody] DeleteCartDto request)
        {
            var cartId = GetCartId();

            if (!int.TryParse(request.id, out int cartItemId))
            {
                return BadRequest("Invalid cart item ID");
            }

            var cartItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == cartItemId && oi.OrderId == null && oi.CartId == cartId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }

            _context.OrderItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item removed from cart" });
        }

        [HttpPatch("delivery")]
        public async Task<ActionResult<CartItemResponseDto>> UpdateDeliveryOption([FromBody] UpdateDeliveryDto request)
        {
            var cartId = GetCartId();

            if (!int.TryParse(request.id, out int cartItemId))
            {
                return BadRequest("Invalid cart item ID");
            }

            if (!int.TryParse(request.deliveryOption, out int deliveryOptionId))
            {
                return BadRequest("Invalid delivery option ID");
            }

            var cartItem = await _context.OrderItems
                .Include(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                .Include(oi => oi.DeliveryOption)
                .FirstOrDefaultAsync(oi => oi.Id == cartItemId && oi.OrderId == null && oi.CartId == cartId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }

            cartItem.DeliveryOptionId = deliveryOptionId;
            cartItem.UpdatedAt = System.DateTime.Now;
            await _context.SaveChangesAsync();

            // Reload to get updated delivery option
            await _context.Entry(cartItem).Reference(oi => oi.DeliveryOption).LoadAsync();

            return Ok(MapToCartItemDto(cartItem));
        }

        private CartItemResponseDto MapToCartItemDto(OrderItem item)
        {
            return new CartItemResponseDto
            {
                _id = item.Id.ToString(),
                quantity = item.Quantity,
                product = new ProductInCartDto
                {
                    _id = item.Product.Id.ToString(),
                    image = item.Product.Image,
                    name = item.Product.Name,
                    priceCents = (int)(item.Product.Price * 100),
                    description = item.Product.Description,
                    keywords = string.IsNullOrEmpty(item.Product.Keywords)
                        ? new List<string>()
                        : item.Product.Keywords.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList(),
                    rating = new RatingDto
                    {
                        stars = item.Product.RatingStars,
                        count = item.Product.RatingCount
                    }
                },
                deliveryOption = item.DeliveryOption != null ? new DeliveryOptionDto
                {
                    _id = item.DeliveryOption.Id.ToString(),
                    name = item.DeliveryOption.Name,
                    priceCents = item.DeliveryOption.PriceCents,
                    estimatedDays = item.DeliveryOption.EstimatedDays,
                    createdAt = item.DeliveryOption.CreatedAt,
                    updatedAt = item.DeliveryOption.UpdatedAt
                } : null,
                createdAt = item.CreatedAt,
                updatedAt = item.UpdatedAt
            };
        }
    }
}
