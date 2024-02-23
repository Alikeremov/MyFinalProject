using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IReviewService
    {
        Task<ICollection<ReviewItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<ICollection<ReviewItemVm>> GetAllSoftDeletes(int page, int take);
        Task<ReviewItemVm> GetWithoutIsdeletedAsync(int id);
        Task<ReviewItemVm> GetAsync(int id);
        Task<bool> CreateAsync(int id, ReviewCreateVm restaurantVm, ModelStateDictionary modelState);
        Task<ReviewCreateVm> CreatedAsync( ReviewCreateVm vm);
        Task<bool> UpdateAsync(FdCategoryUpdateVm restaurantVm, ModelStateDictionary modelState, int id);
        Task<FdCategoryUpdateVm> UpdatedAsync(FdCategoryUpdateVm restaurantvm, int id);

        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
