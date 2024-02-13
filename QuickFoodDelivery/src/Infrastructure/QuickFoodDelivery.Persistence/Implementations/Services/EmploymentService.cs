using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.Utilites.Extensions;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Domain.Enums;
using QuickFoodDelivery.Persistence.Implementations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class EmploymentService : IEmploymentService
    {
        private readonly IEmploymentRepository _repository;
        private readonly IWebHostEnvironment _env;

        public EmploymentService(IEmploymentRepository repository,IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public async Task<ICollection<EmploymentItemVm>> GetAllSoftDeletes(int page, int take)
        {
            ICollection<Employment> employments = await _repository.GetAllWhere(isDeleted: true, skip: (page - 1) * take, take: take).ToListAsync();
            return employments.Select(employment => new EmploymentItemVm
            {
                Id = employment.Id,
                Tittle=employment.Tittle,
                Description=employment.Description,
                Subtittle=employment.Subtittle,
                ButtonText=employment.ButtonText,
                Image=employment.Image,
            }).ToList();
        }
        public async Task<ICollection<EmploymentItemVm>> GetAllunSoftDeletesAsync(int page, int take)
        {
            ICollection<Employment> employments = await _repository.GetAllWhere(isDeleted: false, skip: (page - 1) * take, take: take).ToListAsync();
            return employments.Select(employment => new EmploymentItemVm
            {
                Id = employment.Id,
                Tittle = employment.Tittle,
                Description = employment.Description,
                Subtittle = employment.Subtittle,
                ButtonText = employment.ButtonText,
                Image = employment.Image,
            }).ToList();
        }

        public async Task<EmploymentItemVm> GetAsync(int id)
        {
            Employment employment = await _repository.GetByIdAsync(id, isDeleted: false);
            if (employment == null) throw new Exception("NotFound");
            return new EmploymentItemVm
            {
                Id = employment.Id,
                Tittle = employment.Tittle,
                Description = employment.Description,
                Subtittle = employment.Subtittle,
                ButtonText = employment.ButtonText,
                Image = employment.Image,
            };
        }
        public async Task<bool> CreateAsync(EmploymentCreateVm employmentvm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;

            if (await _repository.Cheeck(x => x.Tittle == employmentvm.Tittle))
            {
                modelState.AddModelError("Tittle", "You have this tittle");
            }
            Employment employment = new Employment
            {
                Tittle = employmentvm.Tittle,
                Description = employmentvm.Description,
                Subtittle = employmentvm.Subtittle,
                ButtonText = employmentvm.ButtonText,
                IsDeleted=false
            };
            if (employmentvm.Photo != null)
            {
                if (!employmentvm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!employmentvm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }

                employment.Image = await employmentvm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "serviceImages");
            }
            await _repository.AddAsync(employment);
            await _repository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(EmploymentUpdateVm employmentvm, ModelStateDictionary modelState, int id)
        {
            if (!modelState.IsValid) return false;
            if (await _repository.Cheeck(x => x.Tittle == employmentvm.Tittle))
            {
                modelState.AddModelError("Tittle", "You have this tittle");
            }
            Employment existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not found");
            existed.Tittle = employmentvm.Tittle;
            existed.Description = employmentvm.Description;
            existed.Subtittle = employmentvm.Subtittle;
            existed.ButtonText = employmentvm.ButtonText;
            existed.Image=employmentvm.Image;

            if (employmentvm.Photo != null)
            {
                if (!employmentvm.Photo.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!employmentvm.Photo.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }
                string fileName = await employmentvm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "serviceImages");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "serviceImages");
                existed.Image = fileName;
            }
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            return true;
        }


        public async Task<EmploymentUpdateVm> UpdatedAsync(EmploymentUpdateVm employmentvm, int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Employment existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            employmentvm.Tittle = existed.Tittle;
            employmentvm.Description = existed.Description;
            employmentvm.Subtittle = existed.Subtittle;
            employmentvm.ButtonText = existed.ButtonText;
            employmentvm.Image = existed.Image;
            return employmentvm;
        }
        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Employment existed = await _repository.GetByIdnotDeletedAsync(id);
            if (existed == null) throw new Exception("Not Found");
            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "serviceImages");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();
        }


        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Employment existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new Exception("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }
        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Employment existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }
    }
}
