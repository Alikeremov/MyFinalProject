
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;


namespace QuickFoodDelivery.Mvc.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly IOrderService _service;
		private readonly ICourierService _courierService;

		public OrderController(IOrderService service,ICourierService courierService)
        {
            _service = service;
			_courierService = courierService;
		}
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CheckOut()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            return View(await _service.CheckOuted(new OrderCreateVm()));
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderCreateVm orderVM,string stripeEmail,string stripeToken)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            if (!await _service.CheckOut(orderVM,ModelState,TempData, stripeEmail,stripeToken)) return View(await _service.CheckOuted(new OrderCreateVm()));
            return RedirectToAction("FindCourier", "Order",new {id=2});
        }
        public async Task<IActionResult> FindCourier(int id)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("Login", "Account");
            if (id ==2) return View(new List<CourierItemVm>());
            return View(await _courierService.GetCourierForOrder());
        }
    }
}
