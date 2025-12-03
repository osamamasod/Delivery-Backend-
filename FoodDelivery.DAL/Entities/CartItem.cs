using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DAL.Entities;

public class CartItem : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public int Quantity { get; set; } = 1;
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid DishId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Dish Dish { get; set; } = null!;
}