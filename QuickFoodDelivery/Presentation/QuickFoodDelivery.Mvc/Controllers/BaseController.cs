using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Domain.Enums;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHttpContextAccessor _accessor;

        public BaseController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        protected ActionResult RedirectToIndexBasedOnRole()
        {
            if (_accessor.HttpContext.User.IsInRole(UserRole.Admin.ToString()))
            {
                return RedirectToAction("Index", "Dashboard",new {area="manage"});
            }
            else if (_accessor.HttpContext.User.IsInRole(UserRole.Member.ToString()))
            {
                return RedirectToAction("Index", "RestaurantaAdmin");
            }
            else if (_accessor.HttpContext.User.Identity != null && _accessor.HttpContext.User.Identity.Name != null)
            {
                return RedirectToAction("Index", "RestaurantaAdmin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
