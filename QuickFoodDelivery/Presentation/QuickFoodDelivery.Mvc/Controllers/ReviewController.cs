using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;

namespace QuickFoodDelivery.Mvc.Controllers
{
    [Authorize(Roles ="Member")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _service;
        private readonly IRestaurantService _restaurantService;
        
        public ReviewController(IReviewService service,IRestaurantService restaurantService)
        {
            _service = service;
            _restaurantService = restaurantService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(int id,ReviewCreateVm vm)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            if (await _service.CreateAsync(id, vm, ModelState))
            {
                ModelState.Clear();
                return View("~/Views/Restaurant/About.cshtml", new AboutVm { Restaurant = await _restaurantService.GetRestaurantAndReviewVithPaginationAsync(id) });
            }
            return View("~/Views/Restaurant/About.cshtml", new AboutVm { Restaurant = await _restaurantService.GetRestaurantAndReviewVithPaginationAsync(id), ReviewCreateVm = new ReviewCreateVm { RestaurantId = id, Description = null, Quality = 0 } });
        }
    }
}
