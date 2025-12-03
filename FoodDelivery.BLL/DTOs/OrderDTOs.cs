using System.ComponentModel.DataAnnotations;
using FoodDelivery.DAL.Entities;  // Use enums from DAL

namespace FoodDelivery.BLL.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; } = null!;
    }

    public class OrderDetailsDto : OrderDto
    {
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid DishId { get; set; }
        public string DishName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }

    public class OrderCreateDto
    {
        [Required]
        public DateTime DeliveryTime { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = null!;
    }

    // REMOVE OrderStatus enum - use the one from DAL
}