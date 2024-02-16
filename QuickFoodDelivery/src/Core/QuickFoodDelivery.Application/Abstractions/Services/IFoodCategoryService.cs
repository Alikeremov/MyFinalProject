using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IFoodCategoryService
    {
        Task<ICollection<FdCategoryItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<ICollection<FdCategoryItemVm>> GetAllSoftDeletes(int page, int take);
        Task<FdCategoryItemVm> GetWithoutIsdeletedAsync(int id);
        Task<FdCategoryItemVm> GetAsync(int id);
        Task<bool> CreateAsync(FdCategoryCreateVm restaurantVm, ModelStateDictionary modelState);
        //Task<FdCategoryCreateVm> CreatedAsync(FdCategoryCreateVm vm);
        Task<bool> UpdateAsync(FdCategoryUpdateVm restaurantVm, ModelStateDictionary modelState, int id);
        Task<FdCategoryUpdateVm> UpdatedAsync(FdCategoryUpdateVm restaurantvm, int id);

        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
