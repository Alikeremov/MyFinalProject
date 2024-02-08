using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Application.Utilites.Extensions;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Persistence.Implementations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _repository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IFoodCategoryRepository _fdrepository;
        private readonly IWebHostEnvironment _env;

        public MealService(IMealRepository repository, IRestaurantRepository restaurantRepository, IFoodCategoryRepository fdrepository, IWebHostEnvironment env)
        {
            _repository = repository;
            _restaurantRepository = restaurantRepository;
            _fdrepository = fdrepository;
            _env = env;
        }
        public async Task<ICollection<MealItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<Meal> meals = await _repository.GetAllWhere(isDeleted: true, skip: (page - 1) * take, take: take).ToListAsync();
            return meals.Select(meal => new MealItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
                Price = meal.Price,
                Description = meal.Description,
                Image = meal.Image,
                RestaurantId = meal.RestaurantId,
            }).ToList();
        }

        public async Task<ICollection<MealItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            ICollection<Meal> meals = await _repository.GetAllWhere(isDeleted: false, skip: (page - 1) * take, take: take).ToListAsync();
            return meals.Select(meal => new MealItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
                Price = meal.Price,
                Description = meal.Description,
                Image = meal.Image,
                RestaurantId = meal.RestaurantId,
                FoodCategoryId = meal.FoodCategoryId,
            }).ToList();
        }

        public async Task<MealItemVm> GetAsync(int id)
        {
            Meal meal = await _repository.GetByIdAsync(id);
            if (meal == null) throw new Exception("NotFound");
            return new MealItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
                Price = meal.Price,
                Description = meal.Description,
                Image = meal.Image,
                RestaurantId = meal.RestaurantId,
                FoodCategoryId = meal.FoodCategoryId,
            };
        }
        public async Task<bool> CreateAsync(MealCreateVm mealVm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;
            if (!await _restaurantRepository.Cheeck(x => x.Id == mealVm.RestaurantId))
            {
                modelState.AddModelError("RestaurantId", "You dont have this Restaurant");
                return false;
            }
            if (mealVm.FoodCategoryId is not null)
            {
                if (!await _fdrepository.Cheeck(x => x.Id == mealVm.FoodCategoryId))
                {
                    modelState.AddModelError("FoodCategoryId", "You dont have this Food Category");
                    return false;
                }

            }
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(mealVm.RestaurantId, includes: new string[] { nameof(Restaurant.Meals) });
            if (restaurant.Meals.Where(x => x.Name == mealVm.Name && x.RestaurantId == restaurant.Id).Count() >= 1)
            {
                modelState.AddModelError("Name", "You have this meal in your Meals");
                return false;
            }
            Meal meal = new Meal
            {
                Name = mealVm.Name,
                Price = mealVm.Price,
                Description = mealVm.Description,
                RestaurantId = mealVm.RestaurantId,
                FoodCategoryId = mealVm.FoodCategoryId,
            };
            if (mealVm.Photo != null)
            {
                if (!mealVm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!mealVm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }

                meal.Image = await mealVm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "mealImages");
            }
            await _repository.AddAsync(meal);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<MealCreateVm> CreatedAsync(MealCreateVm vm)
        {
            vm.Restaurants = await _restaurantRepository.GetAll().ToListAsync();
            vm.FoodCategories = await _fdrepository.GetAll().ToListAsync();
            return vm;
        }

        public async Task<bool> UpdateAsync(MealUpdateVm mealVm, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            Meal existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not found");
            if (await _restaurantRepository.Cheeck(x => x.Id == mealVm.RestaurantId) == false)
            {
                modelState.AddModelError("RestaurantId", "You dont have this Restaurant");
                return false;
            }
            if (mealVm.FoodCategoryId is not null)
            {
                if (!await _fdrepository.Cheeck(x => x.Id == mealVm.FoodCategoryId))
                {
                    modelState.AddModelError("FoodCategoryId", "You dont have this Food Category");
                    return false;
                }

            }
            if (mealVm.Name != existed.Name)
            {
                ICollection<Meal> meals=await _repository.GetAll().ToListAsync();
                
                if (meals.Where(x => x.Name == mealVm.Name && x.RestaurantId ==mealVm.RestaurantId).Count() >= 1)
                {
                    modelState.AddModelError("Name", "You have this meal in your Meals");
                    return false;
                }

            }
            existed.Name = mealVm.Name;
            existed.RestaurantId = mealVm.RestaurantId;
            existed.Price = mealVm.Price;
            existed.Description = mealVm.Description;

            if (mealVm.Photo != null)
            {
                if (!mealVm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!mealVm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }
                string fileName = await mealVm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "mealImages");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "mealImages");
                existed.Image = fileName;
            }
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<MealUpdateVm> UpdatedAsync(MealUpdateVm mealUpdateVm, int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Meal existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            mealUpdateVm.Restaurants = await _restaurantRepository.GetAll().ToListAsync();
            mealUpdateVm.FoodCategories = await _fdrepository.GetAll().ToListAsync();
            mealUpdateVm.Image = existed.Image;
            mealUpdateVm.Name = existed.Name;
            mealUpdateVm.Description = existed.Description;
            mealUpdateVm.Price = existed.Price;
            mealUpdateVm.RestaurantId = existed.RestaurantId;
            mealUpdateVm.FoodCategoryId = existed.FoodCategoryId;
            return mealUpdateVm;
        }
        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Meal existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "mealImages");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }


        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Meal existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Meal existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

    }
}
