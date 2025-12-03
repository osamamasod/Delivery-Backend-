using System.ComponentModel.DataAnnotations;
using FoodDelivery.DAL.Entities;  // Use enums from DAL

namespace FoodDelivery.BLL.DTOs
{
    public class DishDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsVegetarian { get; set; }
        public double Rating { get; set; }
        public DishCategory Category { get; set; }
    }

    public class DishDetailsDto : DishDto
    {
        public bool CanSetRating { get; set; }
        public int? UserRating { get; set; }
    }

    // REMOVE DishCategory enum - use the one from DAL

    public enum DishSorting
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc,
        RatingAsc,
        RatingDesc
    }

    public class DishQueryDto
    {
        public List<DishCategory>? Categories { get; set; }
        public bool? VegetarianOnly { get; set; }
        public DishSorting? Sorting { get; set; }
    }

    public class SetRatingDto
    {
        [Required]
        [Range(1, 10)]
        public int Value { get; set; }
    }
}