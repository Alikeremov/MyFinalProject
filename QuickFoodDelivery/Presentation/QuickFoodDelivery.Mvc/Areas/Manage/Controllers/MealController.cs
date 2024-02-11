﻿using Microsoft.AspNetCore.Mvc;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;

namespace QuickFoodDelivery.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class MealController : Controller
    {
        private readonly IMealService _service;

        public MealController(IMealService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 3)
        {
            return View(await _service.GetAllunSoftDeletesAsync(page, take));
        }
        public async Task<IActionResult> Arxiv(int page = 1, int take = 3)
        {
            return View(await _service.GetAllSoftDeletes(page, take));
        }
        public async Task<IActionResult> Create()
        {
            return View(await _service.CreatedAsync(new MealCreateVm()));
        }
        [HttpPost]
        public async Task<IActionResult> Create(MealCreateVm createVm)
        {
            if (await _service.CreateAsync(createVm, ModelState)) return RedirectToAction(nameof(Index));
            return View(await _service.CreatedAsync(createVm));
        }
        public async Task<IActionResult> Update(int id)
        {
            return View(await _service.UpdatedAsync(new MealUpdateVm(), id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, MealUpdateVm updateVm)
        {
            if (await _service.UpdateAsync(updateVm, ModelState, id)) return RedirectToAction(nameof(Index));
            return View(await _service.UpdatedAsync(updateVm, id));
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
    }
}