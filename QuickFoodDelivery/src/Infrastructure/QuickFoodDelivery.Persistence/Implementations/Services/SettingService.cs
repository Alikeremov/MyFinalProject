using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repository;

        public SettingService(ISettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<ICollection<SettingItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<Setting> settings = await _repository.GetAllWhere(isDeleted: true, skip: (page - 1) * take, take: take).ToListAsync();
            return settings.Select(setting => new SettingItemVm
            {
                Key = setting.Key,
                Value = setting.Value,
            }).ToList();
        }

        public async Task<ICollection<SettingItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            ICollection<Setting> settings = await _repository.GetAllWhere(isDeleted: false, skip: (page - 1) * take, take: take).ToListAsync();
            return settings.Select(setting => new SettingItemVm
            {
                Key = setting.Key,
                Value = setting.Value,
            }).ToList();
        }

        public async Task<SettingItemVm> GetAsync(int id)
        {
            Setting setting = await _repository.GetByIdAsync(id, isDeleted: false);
            if (setting == null) throw new Exception("NotFound");
            return new SettingItemVm
            {
                Key = setting.Key,
                Value = setting.Value,
            };
        }

        public async Task<SettingItemVm> GetWithoutIsdeletedAsync(int id)
        {
            Setting setting = await _repository.GetByIdnotDeletedAsync(id);
            if (setting == null) throw new Exception("NotFound");
            return new SettingItemVm
            {
                Key = setting.Key,
                Value = setting.Value,
            };
        }
        public async Task<bool> CreateAsync(SettingCreateVm settingvm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;
            if (await _repository.Cheeck(x => x.Key == settingvm.Key))
            {
                modelState.AddModelError("Key", "You have same name key please use another key");
                return false;
            }
            Setting setting = new Setting
            {
                Key = settingvm.Key,
                IsDeleted=false,
                Value = settingvm.Value,
            };
            await _repository.AddAsync(setting);
            await _repository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(SettingUpdateVm settingvm, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            Setting existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not found");
            existed.Value = settingvm.Value;
            existed.IsDeleted = false;
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<SettingUpdateVm> UpdatedAsync(SettingUpdateVm settingvm, int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Setting existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            settingvm.Value = existed.Value;
            return settingvm;
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Setting existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }


        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Setting existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Setting existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }
        

    }
}
