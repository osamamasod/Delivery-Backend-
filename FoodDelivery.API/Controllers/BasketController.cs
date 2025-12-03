using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.API.Controllers
{
    [ApiController]
    [Route("api/basket")]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();
                
            var cart = await _basketService.GetCartAsync(userId.Value);
            return Ok(cart);
        }
        
        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> AddToCart(Guid dishId, [FromBody] AddToCartDto addDto)
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();
                
            try
            {
                await _basketService.AddToCartAsync(userId.Value, dishId, addDto.Quantity);
                return Ok(new { message = "Item added to cart" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        
        
        [HttpDelete("dish/{dishId}")]
public async Task<IActionResult> RemoveFromCart(Guid dishId, [FromQuery] bool increase = true)
{
    var userId = GetCurrentUserId();
    
    if (userId == null)
        return Unauthorized();
        
    try
    {
        await _basketService.RemoveFromCartAsync(userId.Value, dishId, increase);
        return Ok(new { message = increase ? "Quantity decreased" : "Item removed from cart" });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
        
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return null;
                
            return userId;
        }
    }
}