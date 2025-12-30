using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_context;
using projet_API.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projet_API.Controllers
{
    [ApiController]
    [Route("deliverOptions")]  // Matching frontend typo
    public class DeliveryOptionsController : ControllerBase
    {
        private readonly DataContext _context;

        public DeliveryOptionsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryOptionDto>>> GetDeliveryOptions()
        {
            var options = await _context.DeliveryOptions.ToListAsync();

            var optionDtos = options.Select(o => new DeliveryOptionDto
            {
                _id = o.Id.ToString(),
                name = o.Name,
                priceCents = o.PriceCents,
                estimatedDays = o.EstimatedDays,
                createdAt = o.CreatedAt,
                updatedAt = o.UpdatedAt
            }).ToList();

            return Ok(optionDtos);
        }
    }
}
