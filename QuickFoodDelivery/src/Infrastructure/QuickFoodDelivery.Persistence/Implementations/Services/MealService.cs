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
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net.Http;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _repository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IFoodCategoryRepository _fdrepository;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAutenticationService _autenticationService;

        public MealService(IMealRepository repository, IRestaurantRepository restaurantRepository, IFoodCategoryRepository fdrepository, IWebHostEnvironment env, IHttpContextAccessor httpContext, IAutenticationService autenticationService)
        {
            _repository = repository;
            _restaurantRepository = restaurantRepository;
            _fdrepository = fdrepository;
            _env = env;
            _httpContext = httpContext;
            _autenticationService = autenticationService;
        }
        public async Task<PaginateVm<MealItemVm>> GetAllWithPagination(int page, int take)
        {
            if (page <= 0) throw new Exception("Wrong querry");

            int count = await _repository.GetAll(isDeleted:false).CountAsync();

            double totalPage = Math.Ceiling((double)count / take);
            if (totalPage < page - 1) throw new Exception("Wrong querry");
            ICollection<Meal> meals = await _repository.GetAllWhere(isDeleted: false, skip: (page - 1) * take, take: take).ToListAsync();
            ICollection<MealItemVm> mealItemVms = meals.Select(meal => new MealItemVm {Id = meal.Id,Name = meal.Name,Price = meal.Price,Description = meal.Description,Image = meal.Image,RestaurantId = meal.RestaurantId,}).ToList();
            return new PaginateVm<MealItemVm>
            {
                CurrentPage = page,
                TotalPage = totalPage,
                Items = mealItemVms
            };

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
        public async Task<ICollection<MealItemVm>> GetAllUnConfirments(int page, int take)
        {
            ICollection<Meal> meals = await _repository.GetAllWhere(isDeleted: null, skip: (page - 1) * take, take: take).ToListAsync();
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
            Meal meal = await _repository.GetByIdAsync(id, isDeleted: false );
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
        public async Task<MealItemVm> GetwithoutDeleteAsync(int id)
        {
            Meal meal = await _repository.GetByIdnotDeletedAsync(id, includes: new string[] { nameof(Meal.Restaurant), nameof(Meal.FoodCategory) });
            if (meal == null) throw new Exception("NotFound");
            MealItemVm mealItem= new MealItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
                Price = meal.Price,
                Restaurant = new RestaurantItemVm
                {
                    Name = meal.Restaurant.Name,
                    Id = meal.Restaurant.Id,
                },

                Description = meal.Description,
                Image = meal.Image,
                RestaurantId = meal.RestaurantId,
                FoodCategoryId = meal.FoodCategoryId,
            };
            if(meal.FoodCategory != null)
            {
                mealItem.Category = new FdCategoryItemVm
                {
                    Name = meal.FoodCategory.Name,
                    Id = meal.FoodCategory.Id,
                };
            }
            return mealItem;
        }
        public async Task<bool> CreateAsync(MealCreateVm mealVm, ModelStateDictionary modelState)
        {

            if (!modelState.IsValid) return false;
            //if (!await _restaurantRepository.Cheeck(x => x.Id == mealVm.RestaurantId))
            //{
            //    modelState.AddModelError("RestaurantId", "You dont have this Restaurant");
            //    return false;
            //}
            if (mealVm.FoodCategoryId is not null)
            {
                if (!await _fdrepository.Cheeck(x => x.Id == mealVm.FoodCategoryId))
                {
                    modelState.AddModelError("FoodCategoryId", "You dont have this Food Category");
                    return false;
                }

            }

            string username = "";
            if (_httpContext.HttpContext.User.Identity != null)
            {
                username = _httpContext.HttpContext.User.Identity.Name;
            }
            AppUser user = await _autenticationService.GetUserAsync(username);

            Restaurant restaurant = await _restaurantRepository.GetByExpressionAsync(x => x.AppUserId == user.Id, isDeleted: false, includes: new string[] { nameof(Restaurant.Meals) });
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
                FoodCategoryId = mealVm.FoodCategoryId,
            };
            meal.RestaurantId = restaurant.Id;

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
            vm.Restaurants = await _restaurantRepository.GetAll(isDeleted: false).ToListAsync();
            vm.FoodCategories = await _fdrepository.GetAll(isDeleted: false).ToListAsync();
            return vm;
        }

        public async Task<bool> UpdateAsync(MealUpdateVm mealVm, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            Meal existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not found");
            //if (await _restaurantRepository.Cheeck(x => x.Id == mealVm.RestaurantId) == false)
            //{
            //    modelState.AddModelError("RestaurantId", "You dont have this Restaurant");
            //    return false;
            //}
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
                ICollection<Meal> meals = await _repository.GetAllnotDeleted().ToListAsync();

                if (meals.Where(x => x.Name == mealVm.Name && x.RestaurantId == mealVm.RestaurantId).Count() >= 1)
                {
                    modelState.AddModelError("Name", "You have this meal in your Meals");
                    return false;
                }

            }
            existed.Name = mealVm.Name;
            existed.Price = mealVm.Price;
            existed.Description = mealVm.Description;
            existed.IsDeleted = null;

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
            Meal existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            mealUpdateVm.Restaurants = await _restaurantRepository.GetAll(isDeleted: false).ToListAsync();
            mealUpdateVm.FoodCategories = await _fdrepository.GetAll(isDeleted: false).ToListAsync();
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
            Meal existed = await _repository.GetByIdnotDeletedAsync(id);
            if (existed == null) throw new Exception("Not Found");
            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "mealImages");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }


        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Meal existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }
        public async Task Submit(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Meal existed = await _repository.GetByIdAsync(id, isDeleted: null);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Meal existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

    }
}
