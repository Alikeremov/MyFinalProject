using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IServiceService
    {
        Task<ICollection<ServiceItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<ICollection<ServiceItemVm>> GetAllSoftDeletes(int page, int take);
        Task<ServiceItemVm> GetWithoutIsdeletedAsync(int id);
        Task<ServiceItemVm> GetAsync(int id);
        Task<bool> CreateAsync(ServiceCreateVm restaurantVm, ModelStateDictionary modelState);
        Task<bool> UpdateAsync(ServiceUpdateVm restaurantVm, ModelStateDictionary modelState, int id);
        Task<ServiceUpdateVm> UpdatedAsync(ServiceUpdateVm restaurantvm, int id);

        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
