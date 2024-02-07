using Microsoft.AspNetCore.Http;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class MealCreateVm
    {
        [Required(ErrorMessage = "Meal must have name")]
        [MinLength(2, ErrorMessage = "Meal name must contain min 2 caracter")]
        [MaxLength(50, ErrorMessage = "Meal must be max 50 caracter")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Meal must have Description")]
        [MinLength(3, ErrorMessage = "Meal name must contain min 2 caracter")]
        [MaxLength(200, ErrorMessage = "Meal must be max 50 caracter")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Meal must have Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Meal must have restaurant Id")]
        public int RestaurantId { get; set; }
        public int? FoodCategoryId { get; set; }

        [Required(ErrorMessage ="Meal must have photo")]
        public IFormFile Photo { get; set; } = null!;
        public ICollection<Restaurant>? Restaurants { get; set; }
        public ICollection<FoodCategory>? FoodCategories { get; set; }
    }
}
