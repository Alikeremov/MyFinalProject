using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantservice;
        private readonly IFoodCategoryService _foodCategoryService;

        public RestaurantController(IRestaurantService restaurantservice, IFoodCategoryService foodCategoryService)
        {
            _restaurantservice = restaurantservice;
            _foodCategoryService = foodCategoryService;
        }
        public async Task<IActionResult> Details(int id, int? foodcategoryId)
        {
            RestaurantItemVm restaurant = await _restaurantservice.GetAsync(id);
            if (foodcategoryId != null)
            {
                return View(new DetailVm
                {
                    FoodCategories = await _foodCategoryService.GetAllunSoftDeletesAsync(1, 20),
                    Restaurant = restaurant,
                    Meals = restaurant.Meals.Where(x => x.FoodCategoryId == foodcategoryId && x.IsDeleted == false).ToList()
                }); ;
            }
            return View(new DetailVm
            {
                FoodCategories = await _foodCategoryService.GetAllunSoftDeletesAsync(1, 20),
                Restaurant = restaurant,
                Meals = restaurant.Meals.Where(x => x.IsDeleted == false).ToList()
            }); ;
        }
    }
}
