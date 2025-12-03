using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DAL.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        OrderItems = new HashSet<OrderItem>();
    }
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public DateTime OrderTime { get; set; }
    
    [Required]
    public DateTime DeliveryTime { get; set; }
    
    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.InProcess;
    
    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = null!;
    
    // Foreign keys
    public Guid UserId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}

public enum OrderStatus
{
    InProcess,
    Delivered
}