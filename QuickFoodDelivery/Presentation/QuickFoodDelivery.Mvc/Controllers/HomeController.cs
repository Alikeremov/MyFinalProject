using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using System.Diagnostics;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IServiceService _service;
        private readonly ICategoryService _categoryService;
        private readonly IEmploymentService _employmentService;

        public HomeController(IRestaurantService restaurantService,IServiceService service,ICategoryService categoryService,IEmploymentService employmentService)
        {
            _restaurantService = restaurantService;
            _service = service;
            _categoryService = categoryService;
            _employmentService = employmentService;
        }

        public async Task<IActionResult> Index(int page=1,int take=6)
        {
            HomeVm vm = new HomeVm
            {
                RestaurantItems=await _restaurantService.GetAllunSoftDeletesAsync(page,take,isOrdered:true),
                CategoryItems=await _categoryService.GetAllActive(),
                Employments=await _employmentService.GetAllunSoftDeletesAsync(page, take),
                Count=await _restaurantService.GetRestaurantCount()
            };
            return View(vm);
        }
        public async Task<IActionResult> About()
        {
            return View(new AboutUsVm
            {
                Services = await _service.GetAllunSoftDeletesAsync(1, 6)
            });  
        }
        public IActionResult ErrorPage(string error = "wrong happened")
        {
            return View(model: error);
        }
    }
}