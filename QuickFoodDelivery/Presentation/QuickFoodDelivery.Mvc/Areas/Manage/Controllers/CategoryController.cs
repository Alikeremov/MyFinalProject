using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Persistence.DAL;

namespace QuickFoodDelivery.Mvc.Areas.Manage.Controllers
{
	[Area("Manage")]
	[Authorize(Roles ="Admin")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _service;

		public CategoryController(ICategoryService service)
        {
			_service = service;
		}
        public async Task<IActionResult> Index()
		{
			
			return View(await _service.GetAllunSoftDeletesAsync(1, 100));
		}
		public async Task<IActionResult> Detail(int id)
		{
			return View(await _service.GetAsync(id));
		}
		public async Task<IActionResult> Arxiv()
		{
            return View(await _service.GetAllSoftDeletes(1, 100));
        }

        public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CategoryCreateVm vm)
		{
			if (!ModelState.IsValid) return View(vm);
			await _service.Create(vm);
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Update(int id)
		{
			if (id < 1) throw new Exception("Bad Request");
			CategoryItemVm existed=await _service.GetAsync(id);
			if (existed ==null) throw new Exception("Not Found");
			return View(new CategoryUpdateVm { Name=existed.Name});
        }
		[HttpPost]
		public async Task<IActionResult> Update(int id, CategoryUpdateVm vm)
		{
            if (id < 1) throw new Exception("Bad Request");
			await _service.Update(vm, id);
			return RedirectToAction(nameof(Index));
        }
		public async Task<IActionResult> SoftDelete(int id)
		{
            if (id < 1) throw new Exception("Bad Request");
			await _service.SoftDeleteAsync(id);
			return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            await _service.Delete(id);
            return RedirectToAction(nameof(Arxiv));
        }
        public async Task<IActionResult> ReverseDelete(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            await _service.ReverseDelete(id);
            return RedirectToAction(nameof(Arxiv));
        }

    }
}
