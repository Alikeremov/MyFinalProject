using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface ICourierService
    {
        Task<ICollection<CourierItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<ICollection<CourierItemVm>> GetAllnonConfirmed(int page, int take);
        Task<ICollection<CourierItemVm>> GetAllSoftDeletes(int page, int take);
        Task<CourierItemVm> GetbyUserNameAsync(string userName);
        Task<CourierItemVm> GetWithoutIsdeletedAsync(int id);
        Task<CourierItemVm> GetAsync(int id);
        Task<bool> CreateAsync(CourierCreateVm restaurantVm, ModelStateDictionary modelState);
        Task<CourierCreateVm> CreatedAsync(CourierCreateVm vm);
        Task<bool> UpdateAsync(CourierUpdateVm restaurantVm, ModelStateDictionary modelState, int id);
        Task<CourierUpdateVm> UpdatedAsync(CourierUpdateVm restaurantvm, int id);
        Task Submit(int id);

        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
