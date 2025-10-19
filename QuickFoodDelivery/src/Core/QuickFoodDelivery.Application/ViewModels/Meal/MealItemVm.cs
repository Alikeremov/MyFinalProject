using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class MealItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public RestaurantItemVm? Restaurant { get; set; }
        public int? FoodCategoryId { get; set; }
        public FdCategoryItemVm? Category { get; set; }

    }
}
