using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class RestaurantAdminController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IRestaurantService _restaurantService;

        public RestaurantAdminController(IMealService mealService,IRestaurantService restaurantService )
        {
            _mealService = mealService;
            _restaurantService = restaurantService;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity!=null && User.Identity.IsAuthenticated)
            {
                
            }
            RestaurantAdminVm vm = new RestaurantAdminVm
            {
                CreateMealVm = await _mealService.CreatedAsync(new MealCreateVm())
            };
            return View(vm);
        }
        public async Task<IActionResult> CreateYourRestaurant()
        {
            RestaurantCreateVm vm = new RestaurantCreateVm();
            vm = await _restaurantService.CreatedAsync(vm);

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateYourRestaurant(RestaurantCreateVm createVm)
        {
            if (await _restaurantService.CreateAsync(createVm, ModelState)) return RedirectToAction("Index","Home");
            return View(await _restaurantService.CreatedAsync(createVm));
        }
        public async Task<IActionResult> CreateMeal()
        {
            return View(await _mealService.CreatedAsync(new MealCreateVm()));
        }
        [HttpPost]
        public async Task<IActionResult> CreateMeal(MealCreateVm createVm)
        {
            if (await _mealService.CreateAsync(createVm, ModelState)) return RedirectToAction(nameof(Index));
            return View(await _mealService.CreatedAsync(createVm));
        }
    }
}
