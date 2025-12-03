using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DAL.Entities;

public class Rating : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [Range(1, 10)]
    public int Value { get; set; }
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid DishId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Dish Dish { get; set; } = null!;
}