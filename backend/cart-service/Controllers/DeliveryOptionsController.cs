using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cart_service.Data;
using cart_service.DTOs;

namespace cart_service.Controllers;

[ApiController]
[Route("delivery-options")]
public class DeliveryOptionsController : ControllerBase
{
    private readonly CartContext _context;

    public DeliveryOptionsController(CartContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeliveryOptionDto>>> GetDeliveryOptions()
    {
        var options = await _context.DeliveryOptions.ToListAsync();

        var dtos = options.Select(d => new DeliveryOptionDto
        {
            _id = d.Id.ToString(),
            name = d.Name,
            priceCents = d.PriceCents,
            estimatedDays = d.EstimatedDays,
            createdAt = d.CreatedAt,
            updatedAt = d.UpdatedAt
        }).ToList();

        return Ok(dtos);
    }
}
