using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.DAL.Entities;

public class OrderItem : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    // Foreign keys
    public Guid OrderId { get; set; }
    public Guid DishId { get; set; }
    
    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual Dish Dish { get; set; } = null!;
}