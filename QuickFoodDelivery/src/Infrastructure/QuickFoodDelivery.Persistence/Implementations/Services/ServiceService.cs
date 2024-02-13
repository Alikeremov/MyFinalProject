using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repository;

        public ServiceService(IServiceRepository repository)
        {
            _repository = repository;
        }
        public async Task<ICollection<ServiceItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<Service> services = await _repository.GetAllWhere(isDeleted: true, skip: (page - 1) * take, take: take).ToListAsync();
            return services.Select(service => new ServiceItemVm
            {
                Id = service.Id,
                Tittle=service.Tittle,
                Subtittle=service.Subtittle,
                Icon = service.Icon,
            }).ToList();
        }

        public async Task<ICollection<ServiceItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            ICollection<Service> services = await _repository.GetAllWhere(isDeleted: false, skip: (page - 1) * take, take: take).ToListAsync();
            return services.Select(service => new ServiceItemVm
            {
                Id = service.Id,
                Tittle = service.Tittle,
                Subtittle = service.Subtittle,
                Icon = service.Icon,
            }).ToList();
        }

        public async Task<ServiceItemVm> GetAsync(int id)
        {
            Service service = await _repository.GetByIdAsync(id);
            if (service == null) throw new Exception("NotFound");
            return new ServiceItemVm
            {
                Id = service.Id,
                Tittle=service.Tittle,
                Subtittle=service.Subtittle,
                Icon = service.Icon,
            };
        }
        public async Task<bool> CreateAsync(ServiceCreateVm servicevm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;
            if(await _repository.Cheeck(x=>x.Tittle==servicevm.Tittle))
            {
                modelState.AddModelError("Tittle","You have this tittle")
            }

            Service service = new Service
            {
                Tittle= servicevm.Tittle,
                Subtittle= servicevm.Subtittle,
                Icon= servicevm.Icon,
                IsDeleted = false
            };
            await _repository.AddAsync(service);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(ServiceUpdateVm serviceUpdateVm, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            if (await _repository.Cheeck(x => x.Tittle == serviceUpdateVm.Tittle))
            {
                modelState.AddModelError("Tittle", "You have this tittle");
            }
            Service existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not found");
            existed.Tittle = serviceUpdateVm.Tittle;
            existed.Icon = serviceUpdateVm.Icon;
            existed.Subtittle = serviceUpdateVm.Subtittle;
            existed.IsDeleted = false;
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<ServiceUpdateVm> UpdatedAsync(ServiceUpdateVm serviceItemVm, int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Service existed = await _repository.GetByIdAsync(id);
            if (existed == null) throw new Exception("Not Found");
            serviceItemVm.Tittle = existed.Tittle;
            serviceItemVm.Subtittle= existed.Subtittle;
            serviceItemVm.Icon = existed.Icon;
            return serviceItemVm;
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Service existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Service existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Service existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }
    }
}
