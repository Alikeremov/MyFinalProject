using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.Utilites.Extensions;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class AutenticationService : IAutenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public AutenticationService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }


        public async Task<List<string>> Register(RegisterVm vm)
        {
            List<string> str = new List<string>();
            if (!vm.Name.IsLetter())
            {
                str.Add("Your Name or Surname only contain letters");
                return str;
            }
            if (!vm.Email.CheeckEmail())
            {
                str.Add("Your Email type is not true");
                return str;
            }
            vm.Name.Capitalize();
            vm.Surname.Capitalize();
            if (!vm.Gender.CheeckGender())
            {
                str.Add("Your Gender has not have");
                return str;
            }

            AppUser user = new AppUser
            {
                Name = vm.Name,
                UserName = vm.UserName,
                Surname = vm.Surname,
                Email = vm.Email,
                BirthDate = vm.BirthDate,
                Gender = vm.Gender
            };

            if (vm.ProfileImage != null)
            {
                if (!vm.ProfileImage.CheckType("image/"))
                {
                    str.Add("Your photo type is not true.Please use only image");
                    return str;
                }
                if (!vm.ProfileImage.ValidateSize(5 * 1024))
                {
                    str.Add("Your Email size must be max 5mb");
                    return str;
                }
                user.ProfileImage = await vm.ProfileImage.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            IdentityResult result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {

                    str.Add(error.Description);
                }
                return str;
            }
            await _userManager.AddToRoleAsync(user,UserRoles.Member.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);
            return str;

        }
        public async Task<List<string>> Login(LoginVm vm)
        {
            List<string> str = new List<string>();
            AppUser user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
            if(user == null)
            {
                user=await _userManager.FindByNameAsync(vm.UserNameOrEmail);
                if(user == null)
                {
                str.Add("Username, Email or Password was wrong");
                return str;

                }
            }
            var result = await _signInManager.PasswordSignInAsync(user,vm.Password,vm.IsRemembered,true);
            if(result.IsLockedOut)
            {
                str.Add("You have a lot of fail  try that is why you banned please try some minuts late");
                return str;
            }
            if(!result.Succeeded)
            {
                str.Add("Username, Email or Password was wrong");
                return str;
            }
            return str;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task CreateRoleAsync()
        {
            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });

                }
            }
        }
    }
}
