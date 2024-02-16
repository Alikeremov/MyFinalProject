using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Domain.Enums;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAutenticationService _service;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IEmailService _emailService;

        public AccountController(IAutenticationService service,UserManager<AppUser> usermanager,IEmailService emailService)
        {
            _service = service;
            _usermanager = usermanager;
            _emailService = emailService;
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
            if (result.Any(x => x == "Admin"))
            {
                return RedirectToAction("Index","Dashboard",new {area="manage"});
            }
            if (result.Any())
            {
                foreach (var item in result)
                {
                    ModelState.AddModelError(String.Empty, item);
                    return View(vm);
                }
            }
            return RedirectToAction(nameof(RedirectToIndexBasedOnRole));

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
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordvm forgotPasswordvm)
        {
            if (!ModelState.IsValid) return View(forgotPasswordvm);
            AppUser user = await _usermanager.FindByEmailAsync(forgotPasswordvm.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User Not Found");
                return View(forgotPasswordvm);
            }
            string token = await _usermanager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, HttpContext.Request.Scheme);
            await _emailService.SendEmailAsync(user.Email, "Reset Password", link, false);
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> ResetPassword(string userId,string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException("Token or UserId is null");
            var user =await _usermanager.FindByIdAsync(userId);
            if (user == null) throw new ArgumentNullException("User is null");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordvm vm, string userId,string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException("Token or UserId is null");
            if (!ModelState.IsValid)  return View(vm); 
            var user = await _usermanager.FindByIdAsync(userId);
            if (user == null) throw new ArgumentNullException("User is null");
            var identityUser = await _usermanager.ResetPasswordAsync(user, token, vm.NewPassword);
            return RedirectToAction(nameof(Login));
        }
    }
}
