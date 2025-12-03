using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBasketService _basketService;
        private readonly int _minDeliveryTimeMinutes = 30;
        
        public OrderService(
            ApplicationDbContext context, 
            IMapper mapper,
            IBasketService basketService)
        {
            _context = context;
            _mapper = mapper;
            _basketService = basketService;
        }
        
        public async Task<OrderDto> CreateOrderAsync(Guid userId, OrderCreateDto orderDto)
        {
            // Validate delivery time
            var minDeliveryTime = DateTime.UtcNow.AddMinutes(_minDeliveryTimeMinutes);
            if (orderDto.DeliveryTime <= minDeliveryTime)
            {
                throw new ArgumentException($"Delivery time must be at least {_minDeliveryTimeMinutes} minutes from now");
            }
            
            // Get user cart
            var cartItems = await _basketService.GetCartAsync(userId);
            
            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty");
            }
            
            // Create order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderTime = DateTime.UtcNow,
                DeliveryTime = orderDto.DeliveryTime,
                Address = orderDto.Address,
                Status = DAL.Entities.OrderStatus.InProcess  // Fully qualified
            };
            
            await _context.Orders.AddAsync(order);
            
            // Create order items from cart
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    DishId = cartItem.DishId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.DishPrice
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            
            // Clear cart
            await _basketService.ClearCartAsync(userId);
            
            await _context.SaveChangesAsync();
            
            return _mapper.Map<OrderDto>(order);
        }
        
        public async Task<List<OrderDto>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderTime)
                .ToListAsync();
                
            return _mapper.Map<List<OrderDto>>(orders);
        }
        
        public async Task<OrderDetailsDto> GetOrderDetailsAsync(Guid orderId, Guid userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
                
            if (order == null)
                throw new KeyNotFoundException("Order not found");
                
            return _mapper.Map<OrderDetailsDto>(order);
        }
        
        public async Task<bool> ConfirmDeliveryAsync(Guid orderId, Guid userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
                
            if (order == null)
                throw new KeyNotFoundException("Order not found");
                
            if (order.Status != DAL.Entities.OrderStatus.InProcess)  // Fully qualified
            {
                throw new InvalidOperationException("Only orders in process can be confirmed");
            }
            
            order.Status = DAL.Entities.OrderStatus.Delivered;  // Fully qualified
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}