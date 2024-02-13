using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.Utilites.Extensions;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Persistence.Implementations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class FoodCategoryService : IFoodCategoryService
    {
        private readonly IFoodCategoryRepository _repository;

        public FoodCategoryService(IFoodCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<FdCategoryItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<FoodCategory> meals = await _repository.GetAllWhere(isDeleted: true, skip: (page - 1) * take, take: take).ToListAsync();
            return meals.Select(meal => new FdCategoryItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
            }).ToList();
        }

        public async Task<ICollection<FdCategoryItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            ICollection<FoodCategory> meals = await _repository.GetAllWhere(isDeleted: false, skip: (page - 1) * take, take: take).ToListAsync();
            return meals.Select(meal => new FdCategoryItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
            }).ToList();
        }

        public async Task<FdCategoryItemVm> GetAsync(int id)
        {
            FoodCategory meal = await _repository.GetByIdAsync(id,isDeleted: false);
            if (meal == null) throw new Exception("NotFound");
            return new FdCategoryItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
            };
        }
        public async Task<bool> CreateAsync(FdCategoryCreateVm fdVm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;
            if (await _repository.Cheeck(x => x.Name == fdVm.Name))
            {
                modelState.AddModelError("CategoryId", "You dont have this category");
                return false;
            }
            FoodCategory meal = new FoodCategory
            {
                Name = fdVm.Name,
                IsDeleted=false
            };
            await _repository.AddAsync(meal);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(FdCategoryUpdateVm fdVm, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            FoodCategory existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not found");
            if (fdVm.Name != existed.Name)
            {
                if (await _repository.Cheeck(x => x.Name == fdVm.Name))
                {
                    modelState.AddModelError("Name", "You have same name restaurant like this, please change name");
                    return false;
                }
            }

            existed.Name = fdVm.Name;
            existed.IsDeleted = false;
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<FdCategoryUpdateVm> UpdatedAsync(FdCategoryUpdateVm fdVm, int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            FoodCategory existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            fdVm.Name = existed.Name;
            return fdVm;
        }
        //public Task<FdCategoryCreateVm> CreatedAsync(FdCategoryCreateVm vm)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            FoodCategory existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            FoodCategory existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            FoodCategory existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

    }
}
