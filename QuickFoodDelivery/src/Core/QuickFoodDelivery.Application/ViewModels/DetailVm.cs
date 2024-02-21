using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class DetailVm
    {
        public ICollection<FdCategoryItemVm> FoodCategories { get; set; }
        public ICollection<Meal> Meals { get; set; }
        public ICollection<MealItemVm> AllMeals { get; set; }
        public ICollection<BasketItemVm> BasketItems { get; set; }
        public RestaurantItemVm Restaurant { get; set; }
        public int OrderOfRestaurantCount { get; set; }
    }
}
