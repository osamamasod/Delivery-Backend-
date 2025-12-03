using FoodDelivery.BLL.DTOs;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IBasketService
    {
        Task<List<CartItemDto>> GetCartAsync(Guid userId);
        Task AddToCartAsync(Guid userId, Guid dishId, int quantity);
        Task RemoveFromCartAsync(Guid userId, Guid dishId, bool increase); // Changed signature
        Task ClearCartAsync(Guid userId);
    }
}