using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAutenticationService _autenticationService;

        public ProfileController(IAutenticationService autenticationService)
        {
            _autenticationService = autenticationService;
        }
        public async Task<IActionResult> Index()
        {
            
            return View(await _autenticationService.GetUserAsync(User.Identity.Name));
        }
        public async Task<IActionResult> Update(int id)
        {
            return View();
        }
    }
}
