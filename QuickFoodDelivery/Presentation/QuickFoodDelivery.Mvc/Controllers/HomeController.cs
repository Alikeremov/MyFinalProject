using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using System.Diagnostics;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ICategoryService _categoryService;

        public HomeController(IRestaurantService restaurantService,ICategoryService categoryService)
        {
            _restaurantService = restaurantService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page=1,int take=6)
        {
            HomeVm vm = new HomeVm
            {
                RestaurantItems=await _restaurantService.GetAllunSoftDeletesAsync(page,take),
                CategoryItems=await _categoryService.GetAllActive()
            };
            return View(vm);
        }

    }
}