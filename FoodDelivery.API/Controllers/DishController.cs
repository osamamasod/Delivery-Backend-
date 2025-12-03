using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.API.Controllers
{
    [ApiController]
    [Route("api/dish")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetDishes([FromQuery] DishQueryDto query)
        {
            var dishes = await _dishService.GetDishesAsync(query);
            return Ok(dishes);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishById(Guid id)
        {
            var userId = GetCurrentUserId();
            
            try
            {
                var dish = await _dishService.GetDishByIdAsync(id, userId);
                return Ok(dish);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Dish not found" });
            }
        }
        
        [HttpPost("{id}/rating")]
        [Authorize]
        public async Task<IActionResult> SetRating(Guid id, [FromBody] SetRatingDto ratingDto)
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();
                
            try
            {
                var success = await _dishService.SetRatingAsync(id, userId.Value, ratingDto.Value);
                
                if (!success)
                    return BadRequest(new { message = "You can only rate dishes you have ordered" });
                    
                return Ok(new { message = "Rating set successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/rating/check")]
[Authorize]
public async Task<IActionResult> CanSetRating(Guid id)
{
    var userId = GetCurrentUserId();
    
    if (userId == null)
        return Unauthorized();
        
    try
    {
        var canRate = await _dishService.CanSetRatingAsync(id, userId.Value);
        return Ok(new { canSetRating = canRate });
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