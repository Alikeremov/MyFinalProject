using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
	public interface ICategoryService
	{
		Task<ICollection<CategoryItemVm>> GetAllunSoftDeletesAsync(int page, int take);
		Task<ICollection<CategoryItemVm>> GetAllActive();
        Task<ICollection<CategoryItemVm>> GetAllSoftDeletes(int page, int take);
		Task<CategoryItemVm> GetAsync(int id);
        Task Create(CategoryCreateVm categoryVm);
		Task Update(CategoryUpdateVm categoryVm, int id);
		Task SoftDeleteAsync(int id);
		Task ReverseDelete(int id);
		Task Delete(int id);
	}
}
