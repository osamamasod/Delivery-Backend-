using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        try
        {
            var result = await _userService.RegisterAsync(registerDto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        try
        {
            var result = await _userService.LoginAsync(loginDto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
    
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // With JWT, logout is client-side (token disposal)
        return Ok(new { message = "Logged out successfully" });
    }
    
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        
        if (userId == null)
            return Unauthorized();
            
        try
        {
            var profile = await _userService.GetProfileAsync(userId.Value);
            return Ok(profile);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
    
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto updateDto)
    {
        var userId = GetCurrentUserId();
        
        if (userId == null)
            return Unauthorized();
            
        try
        {
            var profile = await _userService.UpdateProfileAsync(userId.Value, updateDto);
            return Ok(profile);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
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