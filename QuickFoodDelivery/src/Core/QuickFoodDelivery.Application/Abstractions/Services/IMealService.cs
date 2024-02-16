using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IMealService
    {
        Task<ICollection<MealItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<ICollection<MealItemVm>> GetAllSoftDeletes(int page, int take);
        Task<ICollection<MealItemVm>> GetAllUnConfirments(int page, int take);
        Task<MealItemVm> GetwithoutDeleteAsync(int id);
        Task<MealItemVm> GetAsync(int id);
        Task<bool> CreateAsync(MealCreateVm restaurantVm, ModelStateDictionary modelState);
        Task<MealCreateVm> CreatedAsync(MealCreateVm vm);
        Task<bool> UpdateAsync(MealUpdateVm restaurantVm, ModelStateDictionary modelState, int id);
        Task<MealUpdateVm> UpdatedAsync(MealUpdateVm restaurantvm, int id);
        Task Submit(int id);
        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
