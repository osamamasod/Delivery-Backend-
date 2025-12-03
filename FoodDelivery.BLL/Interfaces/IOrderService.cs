using FoodDelivery.BLL.DTOs;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(Guid userId, OrderCreateDto orderDto);
        Task<List<OrderDto>> GetUserOrdersAsync(Guid userId);
        Task<OrderDetailsDto> GetOrderDetailsAsync(Guid orderId, Guid userId);
        Task<bool> ConfirmDeliveryAsync(Guid orderId, Guid userId);
    }
}