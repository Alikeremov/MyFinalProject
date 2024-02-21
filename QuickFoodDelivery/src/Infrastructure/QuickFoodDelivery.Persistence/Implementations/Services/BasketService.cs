using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class BasketService:IBasketService
    {
        private readonly IBasketItemRepository _repository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMealRepository _mealRepository;
        private readonly IAutenticationService _autentication;

        public BasketService(IBasketItemRepository repository,IHttpContextAccessor accessor,IMealRepository mealRepository,IAutenticationService autentication)
        {
            _repository = repository;
            _accessor = accessor;
            _mealRepository = mealRepository;
            _autentication = autentication;
        }
        public async Task<ICollection<BasketItemVm>> GetBasketItems()
        {
            ICollection<BasketItemVm> basketItemsVm=new List<BasketItemVm>();
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);
                ICollection<BasketItem> basketItems =await _repository.GetAllWhere(x => x.AppUserId == user.Id,includes: new string[] { nameof(BasketItem.Meal) }).ToListAsync();
                basketItemsVm= basketItems.Select(basketItem => new BasketItemVm { 
                    Id = basketItem.Id,
                    Price=basketItem.Price,
                    Count=basketItem.Count,
                    Name=basketItem.MealName,
                    AppUserId=basketItem.AppUserId,
                    MealId=basketItem.MealId,
                    }).ToList();
            }
            return basketItemsVm;
        }
        public async Task<int> GetRestaurantCountofBasketItems(ICollection<BasketItemVm> basketItems)
        {
            ICollection<int> restaurantscount = new List<int>();
            foreach (var item in basketItems)
            {
                Meal meal = await _mealRepository.GetByIdAsync(item.MealId, isDeleted: false);
                if (!restaurantscount.Any(x => x == meal.RestaurantId))
                {
                    restaurantscount.Add(meal.RestaurantId);
                }
            }
            return restaurantscount.Count;
        }
        public async Task AddBasket(int id)
        {
            if (id <= 0) throw new Exception("Wrong querry");
            Meal meal = await _mealRepository.GetByIdAsync(id,isDeleted:false);
            if (meal == null) throw new Exception("Meal not found :(");

            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User not found:(");
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.MealId == meal.Id);
                if (item == null)
                {
                    item = new BasketItem
                    {

                        IsDeleted = false,
                        AppUserId = user.Id,
                        MealId = meal.Id,
                        MealName=meal.Name,
                        Count = 1,
                        Price = meal.Price,
                    };
                    user.BasketItems.Add(item);
                }
                else
                {
                    item.Count++;
                }
                await _repository.SaveChangesAsync();
            }


        }
        public async Task Remove(int id)
        {
            if (id <= 0) throw new Exception("Wrong querry");
            Meal meal = await _mealRepository.GetByIdAsync(id,isDeleted:false);
            if (meal == null) throw new Exception("Product Not Found:(");
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User Not Found:(");
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.MealId == meal.Id);
                if (item == null) throw new Exception("Item Not Found:(");
                user.BasketItems.Remove(item);
                await _repository.SaveChangesAsync();
            }
        }
        public async Task Minus(int id)
        {
            if (id <= 0) throw new Exception("Wrong querry");
            Meal meal = await _mealRepository.GetByIdAsync(id, isDeleted: false);
            if (meal == null) throw new Exception("Product Not Found:(");
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User Not Found:(");
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.MealId == meal.Id);
                if (item == null) throw new Exception("Item Not Found:(");
                item.Count--;
                _repository.Update(item);
                await _repository.SaveChangesAsync();
            }
        }
        public async Task Plus(int id)
        {
            if (id <= 0) throw new Exception("Wrong querry");
            Meal meal = await _mealRepository.GetByIdAsync(id, isDeleted: false);
            if (meal == null) throw new Exception("Product Not Found:(");
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User Not Found:(");
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.MealId == meal.Id);
                if (item == null) throw new Exception("Item Not Found:(");
                item.Count++;
                _repository.Update(item);
                await _repository.SaveChangesAsync();
            }
        }


    }
}
