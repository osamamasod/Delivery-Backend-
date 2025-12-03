using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();
                
            var orders = await _orderService.GetUserOrdersAsync(userId.Value);
            return Ok(orders);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(Guid id)
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();
                
            try
            {
                var order = await _orderService.GetOrderDetailsAsync(id, userId.Value);
                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Order not found" });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();
                
            try
            {
                var order = await _orderService.CreateOrderAsync(userId.Value, orderDto);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("{id}/status")]
        public async Task<IActionResult> ConfirmDelivery(Guid id)
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();
                
            try
            {
                var success = await _orderService.ConfirmDeliveryAsync(id, userId.Value);
                
                if (!success)
                    return BadRequest(new { message = "Failed to confirm delivery" });
                    
                return Ok(new { message = "Delivery confirmed" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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