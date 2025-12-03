using FoodDelivery.BLL.DTOs;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponseDto> RegisterAsync(UserRegisterDto registerDto);
        Task<TokenResponseDto> LoginAsync(UserLoginDto loginDto);
        Task<UserProfileDto> GetProfileAsync(Guid userId);
        Task<UserProfileDto> UpdateProfileAsync(Guid userId, UserUpdateDto updateDto);
    }
}