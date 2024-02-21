using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface ISettingService
    {
        Task<ICollection<SettingItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<ICollection<SettingItemVm>> GetAllSoftDeletes(int page, int take);
        Task<SettingItemVm> GetWithoutIsdeletedAsync(int id);
        Task<SettingItemVm> GetAsync(int id);
        Task<bool> CreateAsync(SettingCreateVm settingvm, ModelStateDictionary modelState);
        //Task<FdCategoryCreateVm> CreatedAsync(FdCategoryCreateVm vm);
        Task<bool> UpdateAsync(SettingUpdateVm settingvm, ModelStateDictionary modelState, int id);
        Task<SettingUpdateVm> UpdatedAsync(SettingUpdateVm settingvm, int id);

        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
