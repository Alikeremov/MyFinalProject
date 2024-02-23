using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;

namespace QuickFoodDelivery.Mvc.Controllers
{
    [Authorize(Roles = "Courier,Member,Admin,RestaurantAdmin")]
    public class ProfileController : Controller
    {
        private readonly IAutenticationService _autenticationService;
        private readonly IOrderService _orderService;

        public ProfileController(IAutenticationService autenticationService, IOrderService orderService)
        {
            _autenticationService = autenticationService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            return View(await _autenticationService.GetUserAsync(User.Identity.Name));
        }
        public async Task<IActionResult> Edit()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            return View(await _autenticationService.Updated(User.Identity.Name, new ProfileUpdateVm()));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProfileUpdateVm vm)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            if (!await _autenticationService.Update(User.Identity.Name, vm, ModelState)) return View(await _autenticationService.Updated(User.Identity.Name, vm));
            await _autenticationService.Logout();

            await _autenticationService.LoginNoPass(vm.UserName, ModelState);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MyOrders()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            return View(await _orderService.GetAllOrdersByUserName(User.Identity.Name));
        }
        public async Task<IActionResult> MyDeliveredOrders()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            return View(await _orderService.GetAllDeliveredOrdersByUserName(User.Identity.Name));
        }
        public async Task<IActionResult> OrderDetail(int id)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            return View(await _orderService.GetOrderById(id));
        }
    }
}
