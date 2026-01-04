using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using catalog_service.Data;
using catalog_service.Models;
using catalog_service.DTOs;

namespace catalog_service.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly CatalogContext _context;

    public ProductsController(CatalogContext context)
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
                        : p.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            type = p.Category?.Name,
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
                        : product.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            type = product.Category?.Name,
            sizeChartLink = "images/clothing-size-chart.png"
        };

        return Ok(productDto);
    }
}
