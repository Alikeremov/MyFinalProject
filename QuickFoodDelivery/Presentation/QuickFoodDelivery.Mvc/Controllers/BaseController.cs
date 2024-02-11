using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Domain.Enums;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected ActionResult RedirectToIndexBasedOnRole()
        {
            if (User.IsInRole(UserRoles.Admin.ToString()))
            {
                return RedirectToAction("Index", "Dashboard",new {area="manage"});
            }
            else if (User.IsInRole(UserRoles.Member.ToString()))
            {
                return RedirectToAction("Index", "RestaurantaAdmin");
            }
            else if (User.Identity!=null && User.Identity.Name != null)
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
