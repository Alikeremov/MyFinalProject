using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IEmploymentService
    {
        Task<ICollection<EmploymentItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<ICollection<EmploymentItemVm>> GetAllSoftDeletes(int page, int take);
        Task<EmploymentItemVm> GetWithoutIsdeletedAsync(int id);
        Task<EmploymentItemVm> GetAsync(int id);
        Task<bool> CreateAsync(EmploymentCreateVm restaurantVm, ModelStateDictionary modelState);
        Task<bool> UpdateAsync(EmploymentUpdateVm restaurantVm, ModelStateDictionary modelState, int id);
        Task<EmploymentUpdateVm> UpdatedAsync(EmploymentUpdateVm restaurantvm, int id);
        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);

    }
}
