using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.DAL.Entities;

public class Dish : BaseEntity
{
    public Dish()
    {
        OrderItems = new HashSet<OrderItem>();
        CartItems = new HashSet<CartItem>();
        Ratings = new HashSet<Rating>();
    }
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    [Required]
    public decimal Price { get; set; }
    
    public string ImageUrl { get; set; } = null!;
    
    [Required]
    public bool IsVegetarian { get; set; }
    
    public double Rating { get; set; } = 0;
    
    [Required]
    public DishCategory Category { get; set; }
    
    // Navigation properties
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }
}

public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Dessert,
    Drink
}