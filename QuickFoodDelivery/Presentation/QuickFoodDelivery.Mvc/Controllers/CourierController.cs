using Humanizer;
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
                Orders=await _orderService.AcceptOrders(User.Identity.Name)
            });
        }
        public async Task<IActionResult> DeliveredFoods()
        {
            return View(new CourierIndexVm
            {
                Courier = await _courierService.GetbyUserNameAsync(User.Identity.Name),
                Orders = await _orderService.DeliveredOrders(User.Identity.Name)
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
        public async Task<IActionResult> ChangeOrderStatus(int id)
        {
            return View(await _orderService.Updated(id, new OrderUpdateVm()));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatus(int id, OrderUpdateVm vm)
        {
            if(await _orderService.Update(id, vm,ModelState)) return RedirectToAction(nameof(Index));
            return View(await _orderService.Updated(id,vm));    
        }
    }
}
