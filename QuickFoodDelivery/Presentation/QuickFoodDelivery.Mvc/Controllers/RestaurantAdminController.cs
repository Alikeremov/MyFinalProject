using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class RestaurantAdminController : Controller
    {
        private readonly IMealService _service;

        public RestaurantAdminController(IMealService service )
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity!=null && User.Identity.IsAuthenticated)
            {
                
            }
            RestaurantAdminVm vm = new RestaurantAdminVm
            {
                CreateMealVm = await _service.CreatedAsync(new MealCreateVm())
            };
            return View(vm);
        }
    }
}
