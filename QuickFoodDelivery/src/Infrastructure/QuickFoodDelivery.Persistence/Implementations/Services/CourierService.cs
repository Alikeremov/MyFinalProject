using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Utilites.Extensions;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Persistence.Implementations.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using QuickFoodDelivery.Domain.Enums;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _repository;
		private readonly IOrderRepository _orderRepository;
		private readonly IAutenticationService _autenticationService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;

        public CourierService(ICourierRepository repository,IOrderRepository orderRepository,IAutenticationService autenticationService,IWebHostEnvironment env,IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
			_orderRepository = orderRepository;
			_autenticationService = autenticationService;
            _env = env;
            _contextAccessor = contextAccessor;
        }

		public async Task<ICollection<CourierItemVm>> GetCourierForOrder(ITempDataDictionary tempdata)
		{
			int orderId = 0;
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                orderId = (int)_contextAccessor.HttpContext.Session.GetInt32("OrderId");
            }
            //orderId=(int)tempdata["OrderId"];
			Order order = await _orderRepository.GetByIdAsync(orderId, isDeleted: false);
			if (order == null) throw new Exception("Order Not Found");
            //tempdata.Remove("OrderId");
			ICollection<Courier> couriers= await _repository.GetAll(isDeleted: false, includes: new string[] { nameof(Courier.Orders)}).Take(1).ToListAsync();
			order.CourierId = couriers.FirstOrDefault()?.Id;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
			return couriers.Select(courier => new CourierItemVm
			{
				Id = courier.Id,			
				Name = courier.Name,
				Surname = courier.Surname,
				Email = courier.Email,
				PhoneNumber = courier.PhoneNumber,
				Fee = courier.Fee,
				Image = courier.Image,
				AppUserId = courier.AppUserId,

			}).ToList();
            
		}
		public async Task<ICollection<CourierItemVm>> GetAllnonConfirmed(int page, int take)
        {
            ICollection<Courier> couriers = await _repository.GetAllWhere(isDeleted: null, skip: (page - 1) * take, take: take).ToListAsync();
            return couriers.Select(courier => new CourierItemVm
            {
                Id = courier.Id,
                Name = courier.Name,
                Surname = courier.Surname,
                Email = courier.Email,
                PhoneNumber = courier.PhoneNumber,
                Fee=courier.Fee,
                Image = courier.Image,
                AppUserId=courier.AppUserId
            }).ToList();
        }

        public async Task<ICollection<CourierItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<Courier> couriers = await _repository.GetAllWhere(isDeleted: false, skip: (page - 1) * take, take: take).ToListAsync();
            return couriers.Select(courier => new CourierItemVm
            {
                Id = courier.Id,
                Name = courier.Name,
                Surname = courier.Surname,
                Email = courier.Email,
                PhoneNumber = courier.PhoneNumber,
                Fee = courier.Fee,
                Image = courier.Image,
                AppUserId = courier.AppUserId
            }).ToList();
        }

        public async Task<ICollection<CourierItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            ICollection<Courier> couriers = await _repository.GetAllWhere(isDeleted: true, skip: (page - 1) * take, take: take).ToListAsync();
            return couriers.Select(courier => new CourierItemVm
            {
                Id = courier.Id,
                Name = courier.Name,
                Surname = courier.Surname,
                Email = courier.Email,
                PhoneNumber = courier.PhoneNumber,
                Fee = courier.Fee,
                Image = courier.Image,
                AppUserId = courier.AppUserId
            }).ToList();
        }
        public async Task<CourierItemVm> GetWithoutIsdeletedAsync(int id)
        {
            Courier courier = await _repository.GetByIdAsync(id, isDeleted: false);
            if (courier == null) throw new Exception("NotFound");
            return new CourierItemVm
            {
                Id = courier.Id,
                Name = courier.Name,
                Surname = courier.Surname,
                Email = courier.Email,
                PhoneNumber = courier.PhoneNumber,
                Fee = courier.Fee,
                Image = courier.Image,
                AppUserId = courier.AppUserId
            };
        }
        public async Task<CourierItemVm> GetAsync(int id)
        {
            Courier courier = await _repository.GetByIdAsync(id, isDeleted: false);
            if (courier == null) throw new Exception("NotFound");
            return new CourierItemVm
            {
                Id = courier.Id,
                Name = courier.Name,
                Surname = courier.Surname,
                Email = courier.Email,
                PhoneNumber = courier.PhoneNumber,
                Fee = courier.Fee,
                Image = courier.Image,
                AppUserId = courier.AppUserId
            };
        }

        public async Task<CourierItemVm> GetbyUserNameAsync(string userName)
        {
            string username = _contextAccessor.HttpContext.User.Identity.Name;
            AppUser user = await _autenticationService.GetUserAsync(userName);
            if (userName == null) throw new Exception("User not found");
            Courier courier = await _repository.GetByExpressionAsync(x => x.AppUserId == user.Id, isDeleted: false);
            if (courier == null)
            {
                courier = await _repository.GetByExpressionAsync(x => x.AppUserId == user.Id, isDeleted: null);
                if (courier == null) throw new Exception("Restaurant not found");
            }

            return new CourierItemVm
            {
                Id = courier.Id,
                Name = courier.Name,
                Surname = courier.Surname,
                Email = courier.Email,
                PhoneNumber = courier.PhoneNumber,
                Fee = courier.Fee,
                Image = courier.Image,
                AppUserId = courier.AppUserId

            };
        }
        public async Task<bool> CreateAsync(CourierCreateVm courierCreateVm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;
            Courier courier = new Courier
            {
                Name = courierCreateVm.Name,
                IsDeleted = null,
                Surname = courierCreateVm.Surname,
                Email = courierCreateVm.Email,
                PhoneNumber = courierCreateVm.PhoneNumber,
                Fee = courierCreateVm.Fee,
            };
            if (_contextAccessor.HttpContext.User.Identity != null)
            {
                courier.AppUserId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            courier.Status = CourierStatus.Idle;
            if (courierCreateVm.Photo != null)
            {
                if (!courierCreateVm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!courierCreateVm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }

                courier.Image = await courierCreateVm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "courierImages");
            }
            await _repository.AddAsync(courier);
            await _repository.SaveChangesAsync();
            return true;
        }

        public Task<CourierCreateVm> CreatedAsync(CourierCreateVm vm)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateAsync(CourierUpdateVm couriervm, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            Courier existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not found");
            existed.Name = couriervm.Name;
            existed.IsDeleted = null;
            existed.Surname = couriervm.Surname;
            existed.Email = couriervm.Email;
            existed.PhoneNumber= couriervm.PhoneNumber;
            existed.Fee = couriervm.Fee;
            existed.Image = couriervm.Image;
            if (couriervm.Photo != null)
            {
                if (!couriervm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!couriervm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }
                string fileName = await couriervm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "courierImages");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "courierImages");
                existed.Image = fileName;
            }
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<CourierUpdateVm> UpdatedAsync(CourierUpdateVm restaurantvm, int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Courier existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            restaurantvm.Image = existed.Image;
            restaurantvm.Surname = existed.Surname;
            restaurantvm.Name = existed.Name;
            restaurantvm.PhoneNumber = existed.PhoneNumber;
            restaurantvm.CourierStatus = existed.Status;
            restaurantvm.Email = existed.Email;
            restaurantvm.Fee = existed.Fee;
            return restaurantvm;
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Courier existed =await _repository.GetByIdnotDeletedAsync(id);
            if (existed == null) throw new Exception("Not Found");
            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "courierImages");
            await _autenticationService.UpdateUserRole(existed.AppUserId, UserRole.Member.ToString());
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }
        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Courier existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Courier existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task Submit(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Courier existed = await _repository.GetByIdAsync(id, isDeleted: null);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _autenticationService.UpdateUserRole(existed.AppUserId, UserRole.Courier.ToString());
            await _repository.SaveChangesAsync();
        }


    }
}
