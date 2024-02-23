using Microsoft.AspNetCore.Authorization;
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
		private readonly ICategoryService _categoryService;
		private readonly IMealService _mealService;
        private readonly IFoodCategoryService _foodCategoryService;
        private readonly IBasketService _basketService;

        public RestaurantController(IRestaurantService restaurantservice,ICategoryService categoryService,IMealService mealService, IFoodCategoryService foodCategoryService,IBasketService basketService)
        {
            _restaurantservice = restaurantservice;
			_categoryService = categoryService;
			_mealService = mealService;
            _foodCategoryService = foodCategoryService;
            _basketService = basketService;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 20, string? searchitem = null, int? order = null, int? categoryId = null)
        {
            if (searchitem != null || order != null || categoryId != null)
            {
                return View(new RestaurantIndexVm
                {
                    Categories = await _categoryService.GetAllunSoftDeletesAsync(1, 20),
                    Restaurants = await _restaurantservice.SearchRestaurants(searchitem, order, categoryId),
                    Order = order,
                    SearchItem = searchitem,
                    CategoryId = categoryId
                });
            }
            return View(new RestaurantIndexVm
            {
                Categories = await _categoryService.GetAllunSoftDeletesAsync(1, 20),
                Restaurants = await _restaurantservice.GetAllunSoftDeletesAsync(page, take)
            });
        }
        public async Task<IActionResult> Details(int id, int? foodcategoryId)
        {
            RestaurantItemVm restaurant = await _restaurantservice.GetAsync(id);
            if (foodcategoryId != null)
            {
                return View(new DetailVm
                {
                    BasketItems = await _basketService.GetBasketItems(),
                    FoodCategories = await _foodCategoryService.GetAllunSoftDeletesAsync(1, 20),
                    Restaurant = restaurant,
                    Meals = restaurant.Meals.Where(x => x.FoodCategoryId == foodcategoryId && x.IsDeleted == false).ToList(),
                    AllMeals = await _mealService.GetAllunSoftDeletesAsync(1, 100),
                    OrderOfRestaurantCount = await _basketService.GetRestaurantCountofBasketItems(await _basketService.GetBasketItems())
                });
            }
            return View(new DetailVm
            {
                BasketItems = await _basketService.GetBasketItems(),
                FoodCategories = await _foodCategoryService.GetAllunSoftDeletesAsync(1, 20),
                Restaurant = restaurant,
                Meals = restaurant.Meals.Where(x => x.IsDeleted == false).ToList(),
                AllMeals = await _mealService.GetAllunSoftDeletesAsync(1, 100),
                OrderOfRestaurantCount = await _basketService.GetRestaurantCountofBasketItems(await _basketService.GetBasketItems())
            });
        }
        [Authorize(Roles ="Member,Admin,RestaurantAdmin,Courier")]
        public async Task<IActionResult> About(int id,int page=1,int take=10)
        {

            RestaurantItemVm restaurant = await _restaurantservice.GetAsync(id);
            if(restaurant.Reviews.Count>0)
            {
                restaurant= await _restaurantservice.GetRestaurantAndReviewVithPaginationAsync(id,page,take);
            }
            return View(new AboutVm
            {
                Restaurant = restaurant,
                ReviewCreateVm = new ReviewCreateVm { RestaurantId=id,Quality=0,Description=null}
            });
        }
    }
}
