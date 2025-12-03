using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.BLL.DTOs;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        
        public UserService(
            ApplicationDbContext context, 
            IJwtService jwtService,
            IMapper mapper)
        {
            _context = context;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        
        public async Task<TokenResponseDto> RegisterAsync(UserRegisterDto registerDto)
        {
            // Check if user exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new ArgumentException("User with this email already exists");
            }
            
            // Create new user
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                BirthDate = registerDto.BirthDate,
                Address = registerDto.Address ?? string.Empty,
                PhoneNumber = registerDto.PhoneNumber ?? string.Empty
            };
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            // Generate token
            var token = _jwtService.GenerateToken(user.Id, user.Email);
            
            return new TokenResponseDto { Token = token };
        }
        
        public async Task<TokenResponseDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            
            var token = _jwtService.GenerateToken(user.Id, user.Email);
            return new TokenResponseDto { Token = token };
        }
        
        public async Task<UserProfileDto> GetProfileAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new KeyNotFoundException("User not found");
                
            return _mapper.Map<UserProfileDto>(user);
        }
        
        public async Task<UserProfileDto> UpdateProfileAsync(
            Guid userId, UserUpdateDto updateDto)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new KeyNotFoundException("User not found");
                
            // Update fields if provided
            if (!string.IsNullOrEmpty(updateDto.FullName))
                user.FullName = updateDto.FullName;
                
            if (updateDto.BirthDate.HasValue)
                user.BirthDate = updateDto.BirthDate.Value;
                
            if (!string.IsNullOrEmpty(updateDto.Address))
                user.Address = updateDto.Address;
                
            if (!string.IsNullOrEmpty(updateDto.PhoneNumber))
                user.PhoneNumber = updateDto.PhoneNumber;
                
            await _context.SaveChangesAsync();
            
            return _mapper.Map<UserProfileDto>(user);
        }
    }
}