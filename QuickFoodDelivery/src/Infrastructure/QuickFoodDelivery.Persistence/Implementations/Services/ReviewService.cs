using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAutenticationService _service;
        private readonly IHttpContextAccessor _http;

        public ReviewService(IReviewRepository repository,IRestaurantRepository restaurantRepository,IAutenticationService service,IHttpContextAccessor http)
        {
            _repository = repository;
            _restaurantRepository = restaurantRepository;
            _service = service;
            _http = http;
        }
        public Task<ICollection<ReviewItemVm>> GetAllSoftDeletes(int page, int take)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ReviewItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewItemVm> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewItemVm> GetWithoutIsdeletedAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateAsync(int id,ReviewCreateVm reviewCreateVm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;
            if (reviewCreateVm.Quality <= 0||reviewCreateVm.Quality>5)
            {
                modelState.AddModelError(string.Empty, "Your point must be min 1 and max 5");
                return false;
            }
            AppUser user= await _service.GetUserAsync(_http.HttpContext.User.Identity.Name);
            if (user == null) throw new Exception("User Not found");
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(id, isDeleted: false, includes: new string[] { nameof(Restaurant.Reviews) });
            if (restaurant == null) throw new Exception("Restaurant Not Found");

            Review review = new Review
            {
                Description = reviewCreateVm.Description,
                Quality = reviewCreateVm.Quality,
                RestaurantId = id,
                AppUserId = user.Id,
                IsDeleted = false
            };
            await _repository.AddAsync(review);
            await _repository.SaveChangesAsync();

            int totalQuality = restaurant.Reviews.Sum(r => r.Quality);
            totalQuality += review.Quality;
            int newPopularity = (int)Math.Round((double)totalQuality / (restaurant.Reviews.Count + 1), MidpointRounding.AwayFromZero);
            restaurant.Popularity = newPopularity;
            _restaurantRepository.Update(restaurant);
            await _restaurantRepository.SaveChangesAsync();
            return true;
        }
        public async Task<ReviewCreateVm> CreatedAsync(ReviewCreateVm vm)
        {

            return  new ReviewCreateVm();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task ReverseDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(FdCategoryUpdateVm restaurantVm, ModelStateDictionary modelState, int id)
        {
            throw new NotImplementedException();
        }

        public Task<FdCategoryUpdateVm> UpdatedAsync(FdCategoryUpdateVm restaurantvm, int id)
        {
            throw new NotImplementedException();
        }

    }
}
