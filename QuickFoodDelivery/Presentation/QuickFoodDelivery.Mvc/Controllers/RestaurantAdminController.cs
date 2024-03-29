﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class RestaurantAdminController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IOrderItemService _orderItemService;
        private readonly IRestaurantService _restaurantService;
        private readonly IHttpContextAccessor _accessor;

        public RestaurantAdminController(IMealService mealService,IOrderItemService orderItemService,IRestaurantService restaurantService,IHttpContextAccessor accessor)
        {
            _mealService = mealService;
            _orderItemService = orderItemService;
            _restaurantService = restaurantService;
            _accessor = accessor;
        }
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> Index()
        {
            RestaurantItemVm restaurant=new RestaurantItemVm();
            if (_accessor.HttpContext.User.Identity!=null && _accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                restaurant=await _restaurantService.GetbyUserNameAsync(_accessor.HttpContext.User.Identity.Name);
            }
            RestaurantAdminVm vm = new RestaurantAdminVm
            {
                RestaurantItem= restaurant,
                Meals=restaurant.Meals
            };
            return View(vm);
        }
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> GetAllOrderItems()
        {
            RestaurantItemVm restaurant = new RestaurantItemVm();
            if (_accessor.HttpContext.User.Identity != null && _accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                restaurant = await _restaurantService.GetbyUserNameAsync(_accessor.HttpContext.User.Identity.Name);
            }
            
            return View(await _orderItemService.GetAllByRestaurantId(restaurant.Id));
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
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> UpdateYourRestaurant(int id)
        {
            return View(await _restaurantService.UpdatedAsync(new RestaurantUpdateVm(), id));
        }
        [HttpPost]
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> UpdateYourRestaurant(int id, RestaurantUpdateVm updateVm)
        {
            if (await _restaurantService.UpdateAsync(updateVm, ModelState, id)) return RedirectToAction(nameof(Index));
            return View(await _restaurantService.UpdatedAsync(updateVm, id));
        }
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> CreateMeal()
        {
            return View(await _mealService.CreatedAsync(new MealCreateVm()));
        }
        [HttpPost]
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> CreateMeal(MealCreateVm createVm)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            if (await _mealService.CreateAsync(createVm, ModelState)) return RedirectToAction(nameof(Index));
            return View(await _mealService.CreatedAsync(createVm));
        }
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> UpdateMeal(int id)
        {
            return View(await _mealService.UpdatedAsync(new MealUpdateVm(), id));
        }
        [HttpPost]
        [Authorize(Roles = "RestaurantAdmin")]
        public async Task<IActionResult> UpdateMeal(int id, MealUpdateVm updateVm)
        {
            if (await _mealService.UpdateAsync(updateVm, ModelState, id)) return RedirectToAction(nameof(Index));
            return View(await _mealService.UpdatedAsync(updateVm, id));
        }
    }
}
