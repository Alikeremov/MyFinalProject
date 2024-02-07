using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Application.Utilites.Extensions;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _env;

        public RestaurantService(IRestaurantRepository repository,ICategoryRepository categoryRepository,IWebHostEnvironment env)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _env = env;
        }
        public async Task<ICollection<RestaurantItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<Restaurant> restaurants = await _repository.GetAllWhere(isDeleted:true,skip: (page - 1) * take, take: take).ToListAsync();
            return restaurants.Select(restaurant => new RestaurantItemVm
            { 
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                MinimumOrderAmount = restaurant.MinimumOrderAmount,
                CategoryId = restaurant.CategoryId,
                Image=restaurant.Image,
                RestourantEmail=restaurant.RestourantEmail,
                Phone=restaurant.Phone,
                OpeningTime=restaurant.OpeningTime,
                ClozedTime=restaurant.ClozedTime,
                IsOpening=restaurant.IsOpening,
                LocationCordinate=restaurant.LocationCordinate,
                Meals=restaurant.Meals,
                Reviews=restaurant.Reviews,
            }).ToList();
        }

        public async Task<ICollection<RestaurantItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            ICollection<Restaurant> restaurants = await _repository.GetAllWhere(skip: (page - 1) * take, take: take).ToListAsync();
            return restaurants.Select(restaurant => new RestaurantItemVm
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                MinimumOrderAmount = restaurant.MinimumOrderAmount,
                CategoryId = restaurant.CategoryId,
                Image = restaurant.Image,
                RestourantEmail = restaurant.RestourantEmail,
                Phone = restaurant.Phone,
                OpeningTime = restaurant.OpeningTime,
                ClozedTime = restaurant.ClozedTime,
                IsOpening = restaurant.IsOpening,
                LocationCordinate = restaurant.LocationCordinate,
                Meals = restaurant.Meals,
                Reviews = restaurant.Reviews,
            }).ToList();
        }
        public async Task<RestaurantItemVm> GetAsync(int id)
        {
            Restaurant restaurant = await _repository.GetByIdAsync(id);
            if (restaurant == null) throw new Exception("NotFound");
            return new RestaurantItemVm
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                MinimumOrderAmount = restaurant.MinimumOrderAmount,
                CategoryId = restaurant.CategoryId,
                Image = restaurant.Image,
                RestourantEmail = restaurant.RestourantEmail,
                Phone = restaurant.Phone,
                OpeningTime = restaurant.OpeningTime,
                ClozedTime = restaurant.ClozedTime,
                IsOpening = restaurant.IsOpening,
                LocationCordinate = restaurant.LocationCordinate,
                Meals = restaurant.Meals,
                Reviews = restaurant.Reviews,
            };
        }
        public async Task<bool> CreateAsync(RestaurantCreateVm restaurantvm,ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;

            if (await _repository.Cheeck(x => x.Name == restaurantvm.Name)) throw new Exception("You have this name product please send other name");

            if (!await _categoryRepository.Cheeck(x => x.Id == restaurantvm.CategoryId))
            {
                modelState.AddModelError("CategoryId", "You dont have this category");
                return false;
            }
            Restaurant restaurant = new Restaurant
            { 
                Name = restaurantvm.Name,
                Address = restaurantvm.Address,
                MinimumOrderAmount = restaurantvm.MinimumOrderAmount,
                CategoryId= restaurantvm.CategoryId,
                RestourantEmail= restaurantvm.RestourantEmail,  
                Phone = restaurantvm.Phone,
                OpeningTime = restaurantvm.OpeningTime,
                ClozedTime= restaurantvm.ClozedTime,
                IsOpening= restaurantvm.IsOpening,
                LocationCordinate= restaurantvm.LocationCordinate,
            };
            if (restaurantvm.Photo != null)
            {
                if (!restaurantvm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo","Your photo type is not true.Please use only image");
                    return false;
                }
                if (!restaurantvm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }
                
                restaurant.Image = await restaurantvm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "restaurantImages");
            }
            await _repository.AddAsync(restaurant);
            await _repository.SaveChangesAsync();
            return true;
        }
        public async Task<RestaurantCreateVm> CreatedAsync(RestaurantCreateVm vm)
        {
            vm.Categories=await _categoryRepository.GetAll().ToListAsync();
            return vm;
        }
        public async Task<bool> UpdateAsync(RestaurantUpdateVm restaurantVm, ModelStateDictionary modelState,int id)
        {
            if (!modelState.IsValid) return false;
            Restaurant existed=await _repository.GetByIdAsync(id,isDeleted:false);
            if (existed == null) throw new Exception("Not found");
            if(restaurantVm.Name!=existed.Name)
                if (await _repository.Cheeck(x => x.Name == restaurantVm.Name))
                {
                    modelState.AddModelError("Name", "You have same name restaurant like this, please change name");
                    return false;
                }
            if (await _categoryRepository.Cheeck(x => x.Id == restaurantVm.CategoryId) == false)
            {
                modelState.AddModelError("CategoryId", "You dont have this category");
                return false;
            }
            existed.Name = restaurantVm.Name;
            existed.Address = restaurantVm.Address;
            existed.LocationCordinate = restaurantVm.LocationCordinate;
            existed.MinimumOrderAmount = restaurantVm.MinimumOrderAmount;
            existed.RestourantEmail = restaurantVm.RestourantEmail;
            existed.Phone= restaurantVm.Phone;
            existed.IsOpening = restaurantVm.IsOpening;
            existed.OpeningTime = restaurantVm.OpeningTime;
            existed.ClozedTime = restaurantVm.ClozedTime;
            existed.CategoryId=restaurantVm.CategoryId;

            if (restaurantVm.Photo != null)
            {
                if (!restaurantVm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!restaurantVm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }
                string fileName = await restaurantVm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img","restaurantImages");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "restaurantImages");
                existed.Image = fileName;
            }
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            return true;
        }


        public async Task<RestaurantUpdateVm> UpdatedAsync(RestaurantUpdateVm restaurantvm, int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Restaurant existed=await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            restaurantvm.Categories = await _categoryRepository.GetAll().ToListAsync();
            restaurantvm.Image=existed.Image;
            restaurantvm.Name = existed.Name;
            restaurantvm.Address = existed.Address;
            restaurantvm.LocationCordinate = existed.LocationCordinate;
            restaurantvm.MinimumOrderAmount = existed.MinimumOrderAmount;
            restaurantvm.RestourantEmail = existed.RestourantEmail;
            restaurantvm.Phone = existed.Phone;
            restaurantvm.IsOpening = existed.IsOpening;
            restaurantvm.OpeningTime = existed.OpeningTime;
            restaurantvm.ClozedTime = existed.ClozedTime;
            restaurantvm.CategoryId = existed.CategoryId;
            return restaurantvm;
        }
        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Restaurant existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "restaurantImages");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }



        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Restaurant existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Restaurant existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

    }
}
