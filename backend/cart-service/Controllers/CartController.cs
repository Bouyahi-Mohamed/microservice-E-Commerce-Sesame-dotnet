using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cart_service.Data;
using cart_service.Models;
using cart_service.DTOs;

namespace cart_service.Controllers;

[ApiController]
[Route("cart")]
public class CartController : ControllerBase
{
    private readonly CartContext _context;

    public CartController(CartContext context)
    {
        _context = context;
    }

    private string GetCartId()
    {
        // For simpler migration, we assume getting User ID from claims if authenticated via Gateway/Identity
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return $"user-{userId}";
        }

        // Check header (Gateway might pass this)
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

        var cartItems = await _context.CartItems
            .Where(oi => oi.CartId == cartId)
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

        // Check if product exists (Replica check)
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        // Get default delivery option (Standard)
        var defaultDelivery = await _context.DeliveryOptions
            .FirstOrDefaultAsync(d => d.Name == "Standard Delivery");

        // Check if item already in cart
        var existingItem = await _context.CartItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
            .Include(oi => oi.DeliveryOption)
            .FirstOrDefaultAsync(oi => oi.ProductId == productId && oi.CartId == cartId);

        if (existingItem != null)
        {
            // Update quantity
            existingItem.Quantity += request.quantity;
            existingItem.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(MapToCartItemDto(existingItem));
        }

        // Create new cart item
        var cartItem = new CartItem
        {
            ProductId = productId,
            Quantity = request.quantity,
            DeliveryOptionId = defaultDelivery?.Id,
            CartId = cartId
        };

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        // Reload with includes
        var addedItem = await _context.CartItems
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

        var cartItem = await _context.CartItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
            .Include(oi => oi.DeliveryOption)
            .FirstOrDefaultAsync(oi => oi.Id == cartItemId && oi.CartId == cartId);

        if (cartItem == null)
        {
            return NotFound("Cart item not found");
        }

        cartItem.Quantity = request.quantity;
        cartItem.UpdatedAt = DateTime.Now;
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

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(oi => oi.Id == cartItemId && oi.CartId == cartId);

        if (cartItem == null)
        {
            return NotFound("Cart item not found");
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Item removed from cart" });
    }

    [HttpDelete("clear")]
    public async Task<ActionResult> ClearCart()
    {
        var cartId = GetCartId();
        var items = await _context.CartItems.Where(i => i.CartId == cartId).ToListAsync();
        
        if (items.Any())
        {
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
        
        return Ok(new { message = "Cart cleared" });
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

        var cartItem = await _context.CartItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
            .Include(oi => oi.DeliveryOption)
            .FirstOrDefaultAsync(oi => oi.Id == cartItemId && oi.CartId == cartId);

        if (cartItem == null)
        {
            return NotFound("Cart item not found");
        }

        cartItem.DeliveryOptionId = deliveryOptionId;
        cartItem.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        // Reload to get updated delivery option
        await _context.Entry(cartItem).Reference(oi => oi.DeliveryOption).LoadAsync();

        return Ok(MapToCartItemDto(cartItem));
    }

    [HttpGet("delivery-options")]
    public async Task<ActionResult<IEnumerable<DeliveryOptionDto>>> GetDeliveryOptions()
    {
        var options = await _context.DeliveryOptions.ToListAsync();
        
        var dtos = options.Select(o => new DeliveryOptionDto
        {
            _id = o.Id.ToString(),
            name = o.Name,
            priceCents = o.PriceCents,
            estimatedDays = o.EstimatedDays,
            createdAt = o.CreatedAt,
            updatedAt = o.UpdatedAt
        }).ToList(); // List<DeliveryOptionDto> matches frontend expectation

        return Ok(dtos);
    }

    private CartItemResponseDto MapToCartItemDto(CartItem item)
    {
        return new CartItemResponseDto
        {
            _id = item.Id.ToString(),
            quantity = item.Quantity,
            product = new ProductInCartDto
            {
                _id = item.Product?.Id.ToString() ?? "",
                image = item.Product?.Image ?? "",
                name = item.Product?.Name ?? "",
                priceCents = (int)((item.Product?.Price ?? 0) * 100),
                description = item.Product?.Description ?? "",
                keywords = string.IsNullOrEmpty(item.Product?.Keywords)
                    ? new List<string>()
                    : item.Product.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                rating = new RatingDto
                {
                    stars = item.Product?.RatingStars ?? 0,
                    count = item.Product?.RatingCount ?? 0
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
