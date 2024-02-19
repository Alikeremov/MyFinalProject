using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class CourierController : Controller
    {
        private readonly ICourierService _courierService;
        private readonly IOrderService _orderService;

        public CourierController(ICourierService courierService,IOrderService orderService)
        {
            _courierService = courierService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            return View(new CourierIndexVm
            {
                Courier=await _courierService.GetbyUserNameAsync(User.Identity.Name),
                Orders=await _orderService.AcceptOrders()
            });
        }
        public IActionResult BeCourier()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BeCourier(CourierCreateVm vm)
        {
            if (await _courierService.CreateAsync(vm, ModelState)) return RedirectToAction("Index", "Home");
            return View(vm);
        }
    }
}
