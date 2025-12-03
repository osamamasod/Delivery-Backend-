using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.DAL.Entities;

public class User : BaseEntity
{
    public User()
    {
        Orders = new HashSet<Order>();
        CartItems = new HashSet<CartItem>();
        Ratings = new HashSet<Rating>();
    }
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    
    [Required]
    public string PasswordHash { get; set; } = null!;
    
    public DateTime? BirthDate { get; set; }
    
    [MaxLength(200)]
    public string Address { get; set; } = null!;
    
    [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$")]
    public string PhoneNumber { get; set; } = null!;
    
    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }
}