using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public DishService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<DishDto>> GetDishesAsync(DishQueryDto query)
        {
            var dishesQuery = _context.Dishes.AsQueryable();
            
            // Apply category filter
            if (query.Categories != null && query.Categories.Any())
            {
                dishesQuery = dishesQuery.Where(d => query.Categories.Contains(d.Category));
            }
            
            // Apply vegetarian filter
            if (query.VegetarianOnly.HasValue && query.VegetarianOnly.Value)
            {
                dishesQuery = dishesQuery.Where(d => d.IsVegetarian);
            }
            
            // Apply sorting
            if (query.Sorting.HasValue)
            {
                dishesQuery = query.Sorting.Value switch
                {
                    DishSorting.NameAsc => dishesQuery.OrderBy(d => d.Name),
                    DishSorting.NameDesc => dishesQuery.OrderByDescending(d => d.Name),
                    DishSorting.PriceAsc => dishesQuery.OrderBy(d => d.Price),
                    DishSorting.PriceDesc => dishesQuery.OrderByDescending(d => d.Price),
                    DishSorting.RatingAsc => dishesQuery.OrderBy(d => d.Rating),
                    DishSorting.RatingDesc => dishesQuery.OrderByDescending(d => d.Rating),
                    _ => dishesQuery.OrderBy(d => d.Name)
                };
            }
            
            var dishes = await dishesQuery.ToListAsync();
            return _mapper.Map<IEnumerable<DishDto>>(dishes);
        }
        
        public async Task<DishDetailsDto> GetDishByIdAsync(Guid id, Guid? userId = null)
        {
            var dish = await _context.Dishes.FindAsync(id);
            
            if (dish == null)
                throw new KeyNotFoundException("Dish not found");
                
            var dishDetails = _mapper.Map<DishDetailsDto>(dish);
            
            // Check if user can set rating
            if (userId.HasValue)
            {
                var hasOrdered = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .AnyAsync(oi => oi.DishId == id && 
                                   oi.Order.UserId == userId.Value);
                
                dishDetails.CanSetRating = hasOrdered;
                
                // Get user's rating if exists
                var userRating = await _context.Ratings
                    .FirstOrDefaultAsync(r => r.DishId == id && r.UserId == userId.Value);
                    
                dishDetails.UserRating = userRating?.Value;
            }
            
            return dishDetails;
        }
        
        public async Task<bool> CanSetRatingAsync(Guid dishId, Guid userId)
        {
            // Check if dish exists
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                throw new KeyNotFoundException("Dish not found");
                
            // Check if user has ordered this dish
            var hasOrdered = await _context.OrderItems
                .Include(oi => oi.Order)
                .AnyAsync(oi => oi.DishId == dishId && 
                               oi.Order.UserId == userId);
                               
            return hasOrdered;
        }
        public async Task<bool> SetRatingAsync(Guid dishId, Guid userId, int rating)
        {
            // Check if dish exists
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                throw new KeyNotFoundException("Dish not found");
                
            // Check if user has ordered this dish
            var hasOrdered = await _context.OrderItems
                .Include(oi => oi.Order)
                .AnyAsync(oi => oi.DishId == dishId && 
                               oi.Order.UserId == userId);
                               
            if (!hasOrdered)
                return false;
                
            // Check if rating already exists
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.DishId == dishId && r.UserId == userId);
                
            if (existingRating != null)
            {
                existingRating.Value = rating;
                existingRating.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newRating = new Rating
                {
                    Id = Guid.NewGuid(),
                    DishId = dishId,
                    UserId = userId,
                    Value = rating
                };
                await _context.Ratings.AddAsync(newRating);
            }
            
            // Update dish average rating
            var dishRatings = await _context.Ratings
                .Where(r => r.DishId == dishId)
                .Select(r => r.Value)
                .ToListAsync();
                
            dish.Rating = dishRatings.Any() ? dishRatings.Average() : 0;
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}