using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Enums;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAutenticationService _service;

        public AccountController(IAutenticationService service)
        {
            _service = service;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var result=await _service.Register(vm);
            if(result.Any())
            {
                foreach (var item in result)
                {
                    ModelState.AddModelError(String.Empty, item);
                    return View(vm);
                }
            }
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {

                return View(vm);
            }
            var result = await _service.Login(vm);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    ModelState.AddModelError(String.Empty, item);
                    return View(vm);
                }
            }
            return RedirectToIndexBasedOnRole();
        }
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            await _service.CreateRoleAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
