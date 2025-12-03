namespace FoodDelivery.BLL.DTOs
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid DishId { get; set; }
        public string DishName { get; set; } = null!;
        public decimal DishPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImageUrl { get; set; } = null!;
    }

    // Remove UpdateCartDto and add these:
    public class AddToCartDto
    {
        public Guid DishId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    
}