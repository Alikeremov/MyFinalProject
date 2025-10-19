using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Persistence.Implementations.Services;

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
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> Index()
        {
            return View(new CourierIndexVm
            {
                Courier=await _courierService.GetbyUserNameAsync(User.Identity.Name),
                Orders=await _orderService.AcceptOrders(User.Identity.Name)
            });
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> DeliveredFoods()
        {
            return View(new CourierIndexVm
            {
                Courier = await _courierService.GetbyUserNameAsync(User.Identity.Name),
                Orders = await _orderService.DeliveredOrders(User.Identity.Name)
            });
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> OrderDetail(int id)
        {
            return View(await _orderService.GetOrderById(id));
        }
        [Authorize(Roles ="Member")]
        public IActionResult BeCourier()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> BeCourier(CourierCreateVm vm)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            if (await _courierService.CreateAsync(vm, ModelState)) return RedirectToAction("Index", "Home");
            return View(vm);
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> Update(int id)
        {
            return View(await _courierService.UpdatedAsync(new CourierUpdateVm(), id));
        }
        [HttpPost]
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> Update(int id, CourierUpdateVm updateVm)
        {
            if (await _courierService.UpdateAsync(updateVm, ModelState, id)) return RedirectToAction(nameof(Index));
            return View(await _courierService.UpdatedAsync(updateVm, id));
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> ChangeOrderStatus(int id)
        {
            return View(await _orderService.Updated(id, new OrderUpdateVm()));
        }
        [HttpPost]
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> ChangeOrderStatus(int id, OrderUpdateVm vm)
        {
            if(await _orderService.Update(id, vm,ModelState,Url,Request)) return RedirectToAction(nameof(Index));
            return View(await _orderService.Updated(id,vm));    
        }
    }
}
