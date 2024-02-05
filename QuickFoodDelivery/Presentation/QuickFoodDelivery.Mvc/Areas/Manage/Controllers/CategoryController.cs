using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Persistence.DAL;

namespace QuickFoodDelivery.Mvc.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class CategoryController : Controller
	{
		private readonly AppDbContext _context;

		public CategoryController(AppDbContext context)
        {
			_context = context;
		}
        public IActionResult Index()
		{

			return View();
		}
	}
}
