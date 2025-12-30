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
    [Route("products")] // User requested http://localhost:5000/products/
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();

            var productDtos = products.Select(p => new ProductResponseDto
            {
                _id = p.Id.ToString(),
                image = p.Image,
                name = p.Name,
                description = p.Description,
                rating = new RatingDto
                {
                    stars = p.RatingStars,
                    count = p.RatingCount
                },
                priceCents = (int)(p.Price * 100),
                keywords = string.IsNullOrEmpty(p.Keywords) 
                            ? new List<string>() 
                            : p.Keywords.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList(),
                type = p.Category != null ? p.Category.Name : null,
                sizeChartLink = "images/clothing-size-chart.png"
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category)
                                                  .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductResponseDto
            {
                _id = product.Id.ToString(),
                image = product.Image,
                name = product.Name,
                description = product.Description,
                rating = new RatingDto
                {
                    stars = product.RatingStars,
                    count = product.RatingCount
                },
                priceCents = (int)(product.Price * 100),
                keywords = string.IsNullOrEmpty(product.Keywords)
                            ? new List<string>()
                            : product.Keywords.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList(),
                type = product.Category != null ? product.Category.Name : null,
                sizeChartLink = "images/clothing-size-chart.png"
            };

            return Ok(productDto);
        }
    }
}
