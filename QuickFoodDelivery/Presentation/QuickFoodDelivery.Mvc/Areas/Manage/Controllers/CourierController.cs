using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;

namespace QuickFoodDelivery.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CourierController : Controller
    {
        private readonly ICourierService _service;

        public CourierController(ICourierService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page,int take)
        {
            return View(await _service.GetAllunSoftDeletesAsync(page,take));
        }
        public async Task<IActionResult> Arxiv(int page, int take)
        {
            return View(await _service.GetAllSoftDeletes(page, take));
        }
        public async Task<IActionResult> Detail(int id)
        {
            return View(await _service.GetWithoutIsdeletedAsync(id));
        }
        public async Task<IActionResult> UnConfirments(int page, int take)
        {
            return View(await _service.GetAllnonConfirmed(page, take));
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Arxiv));
        }
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ReverseDelete(int id)
        {
            await _service.ReverseDeleteAsync(id);
            return RedirectToAction(nameof(Arxiv));
        }
        public async Task<IActionResult> Sumbit(int id)
        {
            await _service.Submit(id);
            return RedirectToAction(nameof(UnConfirments));
        }
    }
}
