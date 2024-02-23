using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IRestaurantService
    {
        Task<ICollection<RestaurantItemVm>> GetAllunSoftDeletesAsync(int page, int take);
        Task<PaginateVm<RestaurantItemVm>> GetAllWithPagination(int page, int take);
        Task<ICollection<RestaurantItemVm>> GetAllnonConfirmed(int page, int take);
        Task<ICollection<RestaurantItemVm>> GetAllSoftDeletes(int page, int take);
        Task<RestaurantItemVm> GetbyUserNameAsync(string userName);
        Task<RestaurantItemVm> GetWithoutIsdeletedAsync(int id);
        Task<ICollection<RestaurantItemVm>> SearchRestaurants(string? searchItem, int? order, int? categoryId);
        Task<RestaurantItemVm> GetRestaurantAndReviewVithPaginationAsync(int id, int page = 1, int take = 10);

        Task<RestaurantItemVm> GetAsync(int id);
        Task<bool> CreateAsync(RestaurantCreateVm restaurantVm, ModelStateDictionary modelState);
        Task<RestaurantCreateVm> CreatedAsync(RestaurantCreateVm vm);
        Task<bool> UpdateAsync(RestaurantUpdateVm restaurantVm, ModelStateDictionary modelState, int id);
        Task<RestaurantUpdateVm> UpdatedAsync(RestaurantUpdateVm restaurantvm, int id);
        Task Submit(int id);

        Task SoftDeleteAsync(int id);
        Task ReverseDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
