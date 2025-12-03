using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Services
{
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public BasketService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<List<CartItemDto>> GetCartAsync(Guid userId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Dish)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
                
            return _mapper.Map<List<CartItemDto>>(cartItems);
        }
        
        public async Task AddToCartAsync(Guid userId, Guid dishId, int quantity)
        {
            // Check if dish exists
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                throw new KeyNotFoundException("Dish not found");
                
            // Check if already in cart
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.DishId == dishId);
                
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DishId = dishId,
                    Quantity = quantity
                };
                await _context.CartItems.AddAsync(cartItem);
            }
            
            await _context.SaveChangesAsync();
        }
        
     
        
        public async Task RemoveFromCartAsync(Guid userId, Guid dishId, bool increase)
{
    var cartItem = await _context.CartItems
        .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.DishId == dishId);
        
    if (cartItem == null)
        throw new KeyNotFoundException("Item not found in cart");
        
    if (increase)
    {
        // Decrease quantity by 1 (or remove if quantity becomes 0)
        if (cartItem.Quantity > 1)
        {
            cartItem.Quantity -= 1;
            await _context.SaveChangesAsync();
        }
        else
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
    else
    {
        // Remove completely
        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }
}
        
        public async Task ClearCartAsync(Guid userId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
                
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}