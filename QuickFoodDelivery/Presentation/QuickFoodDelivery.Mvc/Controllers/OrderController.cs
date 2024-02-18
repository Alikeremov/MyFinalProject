
using Microsoft.AspNetCore.Mvc;

using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;


namespace QuickFoodDelivery.Mvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CheckOut()
        {
            return View(await _service.CheckOuted(new OrderCreateVm()));
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderCreateVm orderVM)
        {
            if(!await _service.CheckOut(orderVM,ModelState,TempData)) return View(await _service.CheckOuted(new OrderCreateVm()));
            return RedirectToAction("Index", "Home");
        }
    }
}
