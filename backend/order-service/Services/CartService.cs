using order_service.DTOs;
using System.Text.Json;

namespace order_service.Services;

public interface ICartService
{
    Task<List<ExternalCartItemDto>> GetCartAsync(string userId, string? token = null);
    Task ClearCartAsync(string userId, string? token = null);
}

public class CartService : ICartService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CartService> _logger;

    public CartService(HttpClient httpClient, ILogger<CartService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ExternalCartItemDto>> GetCartAsync(string userId, string? token = null)
    {
        try
        {
            // We assume the token is passed and we forward it
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            
            // If communicating via Gateway or Direct Service
            // Using direct service name "cart-service" (Docker DNS)
            // Path: /cart
            var response = await _httpClient.GetAsync("/cart");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var items = JsonSerializer.Deserialize<List<ExternalCartItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return items ?? new List<ExternalCartItemDto>();
            }
            else
            {
                _logger.LogWarning($"Failed to fetch cart: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching cart");
        }
        
        return new List<ExternalCartItemDto>();
    }

    public async Task ClearCartAsync(string userId, string? token = null)
    {
        // To be implemented: CartService needs a Clear endpoint or we delete items one by one?
        // Current CartController allows DeleteCartItem but not ClearAll.
        // We'll skip clearing for now or loop delete.
        _logger.LogInformation("ClearCartAsync called - Logic to clear cart pending CartService update");
    }
}
