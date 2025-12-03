using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.BLL.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
        
        public string? Address { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$")]
        public string? PhoneNumber { get; set; }
    }

    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;
    }

    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }

    public class UserUpdateDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        [MaxLength(200)]
        public string? Address { get; set; }
        
        [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$")]
        public string? PhoneNumber { get; set; }
    }

    public class TokenResponseDto
    {
        public string Token { get; set; } = null!;
    }
}