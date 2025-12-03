using Microsoft.EntityFrameworkCore;
using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Services
{
    public class SeedService
    {
        private readonly ApplicationDbContext _context;
        
        public SeedService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task SeedDataAsync()
        {
            // Check if data already exists
            if (await _context.Dishes.AnyAsync())
                return;
                
            var dishes = new List<Dish>
            {
                // Wok category
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Chicken Chow Mein",
                    Description = "Stir-fried noodles with chicken and vegetables",
                    Price = 12.99m,
                    ImageUrl = "/images/chowmein.jpg",
                    IsVegetarian = false,
                    Rating = 4.3,
                    Category = DishCategory.Wok
                },
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Vegetable Lo Mein",
                    Description = "Stir-fried noodles with mixed vegetables",
                    Price = 10.99m,
                    ImageUrl = "/images/lomein.jpg",
                    IsVegetarian = true,
                    Rating = 4.5,
                    Category = DishCategory.Wok
                },
                
                // Pizza category
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Margherita Pizza",
                    Description = "Classic pizza with tomato, mozzarella, and basil",
                    Price = 14.99m,
                    ImageUrl = "/images/margherita.jpg",
                    IsVegetarian = true,
                    Rating = 4.7,
                    Category = DishCategory.Pizza
                },
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Pepperoni Pizza",
                    Description = "Pizza with pepperoni and extra cheese",
                    Price = 16.99m,
                    ImageUrl = "/images/pepperoni.jpg",
                    IsVegetarian = false,
                    Rating = 4.6,
                    Category = DishCategory.Pizza
                },
                
                // Soup category
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Tomato Soup",
                    Description = "Creamy tomato soup with herbs",
                    Price = 6.99m,
                    ImageUrl = "/images/tomato-soup.jpg",
                    IsVegetarian = true,
                    Rating = 4.2,
                    Category = DishCategory.Soup
                },
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Chicken Noodle Soup",
                    Description = "Hearty soup with chicken and noodles",
                    Price = 8.99m,
                    ImageUrl = "/images/chicken-noodle.jpg",
                    IsVegetarian = false,
                    Rating = 4.4,
                    Category = DishCategory.Soup
                },
                
                // Dessert category
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Chocolate Cake",
                    Description = "Rich chocolate cake with ganache",
                    Price = 7.99m,
                    ImageUrl = "/images/chocolate-cake.jpg",
                    IsVegetarian = true,
                    Rating = 4.8,
                    Category = DishCategory.Dessert
                },
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Cheesecake",
                    Description = "New York style cheesecake with berry sauce",
                    Price = 8.99m,
                    ImageUrl = "/images/cheesecake.jpg",
                    IsVegetarian = true,
                    Rating = 4.9,
                    Category = DishCategory.Dessert
                },
                
                // Drink category
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Fresh Orange Juice",
                    Description = "Freshly squeezed orange juice",
                    Price = 4.99m,
                    ImageUrl = "/images/orange-juice.jpg",
                    IsVegetarian = true,
                    Rating = 4.5,
                    Category = DishCategory.Drink
                },
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Iced Tea",
                    Description = "Refreshing iced tea with lemon",
                    Price = 3.99m,
                    ImageUrl = "/images/iced-tea.jpg",
                    IsVegetarian = true,
                    Rating = 4.3,
                    Category = DishCategory.Drink
                }
            };
            
            await _context.Dishes.AddRangeAsync(dishes);
            await _context.SaveChangesAsync();
            
            Console.WriteLine("Seed data added successfully!");
        }
    }
}