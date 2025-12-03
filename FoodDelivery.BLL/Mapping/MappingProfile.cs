using AutoMapper;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserProfileDto>();
            CreateMap<UserUpdateDto, User>();
            
            // Dish mappings
            CreateMap<Dish, DishDto>();
            CreateMap<Dish, DishDetailsDto>();
            
            // Cart mappings
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish.Name))
                .ForMember(dest => dest.DishPrice, opt => opt.MapFrom(src => src.Dish.Price))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Dish.Price * src.Quantity));
                
            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => 
                    src.OrderItems.Sum(oi => oi.Price * oi.Quantity)));
                
            CreateMap<Order, OrderDetailsDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => 
                    src.OrderItems.Sum(oi => oi.Price * oi.Quantity)));
                    
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish.Name));
        }
    }
}