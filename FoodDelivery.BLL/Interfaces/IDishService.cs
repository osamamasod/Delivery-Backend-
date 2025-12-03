using FoodDelivery.BLL.DTOs;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IDishService
    {
        Task<IEnumerable<DishDto>> GetDishesAsync(DishQueryDto query);
        Task<DishDetailsDto> GetDishByIdAsync(Guid id, Guid? userId = null);
        Task<bool> SetRatingAsync(Guid dishId, Guid userId, int rating);
        Task<bool> CanSetRatingAsync(Guid dishId, Guid userId);
    }
}