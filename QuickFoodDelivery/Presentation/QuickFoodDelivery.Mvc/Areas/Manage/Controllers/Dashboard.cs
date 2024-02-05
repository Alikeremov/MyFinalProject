using Microsoft.AspNetCore.Mvc;

namespace QuickFoodDelivery.Mvc.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class Dashboard : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
