using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Domain.Enums;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToIndexBasedOnRole()
        {
            if (User.IsInRole(UserRole.Admin.ToString()))
            {
                return RedirectToAction("Index", "Dashboard",new {area="manage"});
            }
            else if (User.IsInRole(UserRole.RestaurantAdmin.ToString()))
            {
                return RedirectToAction("Index", "RestaurantAdmin");
            }
            else if (User.IsInRole(UserRole.Courier.ToString()))
            {
                return RedirectToAction("Index", "Courier");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
