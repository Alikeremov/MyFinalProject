using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Persistence.DAL;
using QuickFoodDelivery.Persistence.Implementations.Repositories;
using QuickFoodDelivery.Persistence.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("default"));
            });
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;

                opt.User.RequireUniqueEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.AllowedForNewUsers = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IRestaurantRepository,RestaurantRepository>();
            services.AddScoped<IFoodCategoryRepository,FoodCategoryRepository>();
            services.AddScoped<IMealRepository,MealRepository>();
            services.AddScoped<IEmploymentRepository,EmploymentRepository>();
            services.AddScoped<IServiceRepository,ServiceRepository>();

            services.AddScoped<IAutenticationService, AutenticationService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFoodCategoryService, FoodCategoryService>();
            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IEmploymentService, EmploymentService>();



            return services;
        }
    }
}
