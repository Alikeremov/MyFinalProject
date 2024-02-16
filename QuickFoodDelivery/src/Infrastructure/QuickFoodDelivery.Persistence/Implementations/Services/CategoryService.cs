using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _repository;

		public CategoryService(ICategoryRepository repository)
        {
			_repository = repository;
		}
		public async Task<ICollection<CategoryItemVm>> GetAllunSoftDeletesAsync(int page, int take)
		{
			ICollection<Category> categories = await _repository.GetAllWhere(includes:new string[] {nameof(Category.Restaurants)},isDeleted:false,skip: (page - 1) * take, take: take).ToListAsync();
			return categories.Select(category => new CategoryItemVm {Id=category.Id, Name = category.Name,Restaurants=category.Restaurants }).ToList();
		}
        public async Task<ICollection<CategoryItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<Category> categories = await _repository.GetAllWhere(includes: new string[] { nameof(Category.Restaurants) },isDeleted:true ,skip: (page - 1) * take, take: take).ToListAsync();
            return categories.Select(category => new CategoryItemVm { Id = category.Id, Name = category.Name, Restaurants = category.Restaurants }).ToList();
        }
        public async Task<ICollection<CategoryItemVm>> GetAllActive()
        {
            ICollection<Category> categories = await _repository.GetAllWhere(includes: new string[] { nameof(Category.Restaurants) }).ToListAsync();
            return categories.Select(category => new CategoryItemVm { Id = category.Id, Name = category.Name, Restaurants = category.Restaurants }).ToList();
        }
        public async Task<CategoryItemVm> GetAsync(int id)
		{
			Category category =await _repository.GetByIdnotDeletedAsync(id);
			if (category == null) throw new Exception("NotFound");
			return new CategoryItemVm {Id=category.Id, Name = category.Name };
		}

		public async Task Create(CategoryCreateVm categoryVm)
		{
			if (await _repository.Cheeck(x => x.Name == categoryVm.Name)) throw new Exception("Bad Request");

			await _repository.AddAsync(new Category { Name=categoryVm.Name,IsDeleted=false});
			await _repository.SaveChangesAsync();
		}
		public async Task Update(CategoryUpdateVm categoryVm, int id)
		{
			Category existed = await _repository.GetByIdnotDeletedAsync(id);
			if (existed == null) throw new Exception("Not Found");
			if (await _repository.Cheeck(x => x.Name == categoryVm.Name)) throw new Exception("Bad Request");
			existed.Name = categoryVm.Name;
			existed.IsDeleted = false;
			_repository.Update(existed);
			await _repository.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			Category existed = await _repository.GetByIdAsync(id,isDeleted:true,includes:new string[] {nameof(Category.Restaurants)});
			if (existed == null) throw new Exception("Not Found");
			
			if (existed.Restaurants.FirstOrDefault() !=null) throw new Exception($"You cant delete this Category because you have this category some restourant if you need delete this category please update them");
			_repository.Delete(existed);		
			await _repository.SaveChangesAsync();
		}


		public async Task ReverseDelete(int id)
		{
			Category existed = await _repository.GetByIdAsync(id, isDeleted:true);
			if (existed == null) throw new Exception("Not Found");
			_repository.ReverseDelete(existed);
			await _repository.SaveChangesAsync();
		}

		public async Task SoftDeleteAsync(int id)
		{
			Category existed = await _repository.GetByIdAsync(id ,isDeleted:false);
			if (existed == null) throw new Exception("Not Found");
			_repository.SoftDelete(existed);
			await _repository.SaveChangesAsync();
		}

    }
}
