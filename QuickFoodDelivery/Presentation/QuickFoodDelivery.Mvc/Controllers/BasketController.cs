using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Persistence.Implementations.Services;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _service;

        public BasketController(IBasketService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddBasket(int mealid,int id)
        {
            await _service.AddBasket(mealid);
            return RedirectToAction("Details", "Restaurant", new {id=id});
        }
        public async Task<IActionResult> Remove(int mealid, int id)
        {
            await _service.Remove(mealid);
            return RedirectToAction("Details", "Restaurant", new { id = id });
        }
        public async Task<IActionResult> Minus(int mealid, int id)
        {
            await _service.Minus(mealid);
            return RedirectToAction("Details", "Restaurant", new { id = id });
        }
        public async Task<IActionResult> Plus(int mealid, int id)
        {
            await _service.Plus(mealid);
            return RedirectToAction("Details", "Restaurant", new { id = id });
        }
    }
}
