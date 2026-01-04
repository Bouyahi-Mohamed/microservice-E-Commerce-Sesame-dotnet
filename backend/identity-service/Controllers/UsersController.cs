using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using identity_service.Data;
using identity_service.Models;
using identity_service.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace identity_service.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IdentityContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(IdentityContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            // Find user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.username);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Verify password
            if (!VerifyPassword(request.password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Get or create customer
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (customer == null)
            {
                customer = new Customer
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Name = user.Username
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            // Generate JWT token
            var token = GenerateJwtToken(user.Id, customer.Id, user.Username);

            // Note: Cart merging logic removed as it belongs to Cart Service. 
            // The frontend should handle merging by sending the token to Cart Service if needed.

            return Ok(new AuthResponseDto
            {
                token = token,
                userId = user.Id,
                customerId = customer.Id,
                username = user.Username,
                email = user.Email,
                name = customer.Name ?? ""
            });
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponseDto>> Signup([FromBody] SignupRequestDto request)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == request.username))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            // Create user
            var user = new User
            {
                Username = request.username,
                Email = request.email,
                PasswordHash = HashPassword(request.password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create customer
            var customer = new Customer
            {
                UserId = user.Id,
                Email = request.email,
                Name = request.name ?? request.username
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = GenerateJwtToken(user.Id, customer.Id, user.Username);

            return Ok(new AuthResponseDto
            {
                token = token,
                userId = user.Id,
                customerId = customer.Id,
                username = user.Username,
                email = user.Email,
                name = customer.Name
            });
        }

        private string GenerateJwtToken(int userId, int customerId, string username)
        {
            var keyString = _configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("customerId", customerId.ToString()),
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "YourApp",
                audience: _configuration["Jwt:Audience"] ?? "YourAppUsers",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}
