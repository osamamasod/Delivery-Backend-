using Microsoft.EntityFrameworkCore;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
            
        // Configure Dish
        modelBuilder.Entity<Dish>()
            .Property(d => d.Price)
            .HasPrecision(18, 2);
            
        // Configure Order
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Configure OrderItem
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Price)
            .HasPrecision(18, 2);
            
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Dish)
            .WithMany(d => d.OrderItems)
            .HasForeignKey(oi => oi.DishId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Configure Rating - unique constraint per user per dish
        modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.UserId, r.DishId })
            .IsUnique();
            
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Dish)
            .WithMany(d => d.Ratings)
            .HasForeignKey(r => r.DishId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Configure CartItem - unique constraint per user per dish
        modelBuilder.Entity<CartItem>()
            .HasIndex(ci => new { ci.UserId, ci.DishId })
            .IsUnique();
            
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.User)
            .WithMany(u => u.CartItems)
            .HasForeignKey(ci => ci.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Dish)
            .WithMany(d => d.CartItems)
            .HasForeignKey(ci => ci.DishId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}