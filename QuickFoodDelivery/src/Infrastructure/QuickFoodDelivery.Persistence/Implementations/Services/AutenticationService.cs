using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
using static System.Net.WebRequestMethods;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class AutenticationService : IAutenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _http;

        public AutenticationService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env,IHttpContextAccessor http)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
            _http = http;
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
                    str.Add("Your Photo size must be max 5mb");
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
            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());
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
            foreach (var cookie in _http.HttpContext.Request.Cookies.Keys)
            {
                _http.HttpContext.Response.Cookies.Delete(cookie);
            }
            await _signInManager.SignOutAsync();
        }
        public async Task<AppUser> GetUserAsync(string userName)
        {
            return await _userManager.Users.Include(x => x.Restaurants).Include(x=>x.BasketItems).Include(x=>x.Orders).Include(x=>x.Couriers).FirstOrDefaultAsync(x=>x.UserName==userName);
        }
        public async Task CreateRoleAsync()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
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
        public async Task<ICollection<AppUser>> GetAllUsers(string searchTerm)
        {
            return await _userManager.Users.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower()) || x.Name.ToLower().Contains(searchTerm.ToLower()) || x.Surname.ToLower().Contains(searchTerm.ToLower())).ToListAsync();
        }
        public async Task UpdateUserRole(string userId,string roleName)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User Not found");
            }
            IList<string> existingRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in existingRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            await _userManager.AddToRoleAsync(user, roleName);
            await _userManager.UpdateAsync(user);
        }
        public async Task<ProfileUpdateVm> Updated(string username,ProfileUpdateVm vm)
        {
            if (username == null) throw new Exception("Bad Request");
            AppUser user = await GetUserAsync(username);
            if (user == null) throw new Exception("Not Found");
            vm.Name=user.Name;
            vm.Surname=user.Surname;
            vm.ProfileImage=user.ProfileImage;
            vm.UserName=user.UserName;
            return vm;
        }
        public async Task<bool> LoginNoPass(string userName,ModelStateDictionary modelState)
        {
            AppUser user = await _userManager.FindByEmailAsync(userName);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(userName);
                if (user is null)
                {
                    modelState.AddModelError(string.Empty, "UserName , email or password was wrong");
                }

            }
            await _signInManager.SignInAsync(user, true);
            return true;
        }
        public async Task<bool> Update(string username,ProfileUpdateVm vm,ModelStateDictionary modelState)
        {
            if(!modelState.IsValid) return false;
            if (username == null) throw new Exception("Bad Request");
            AppUser existed = await GetUserAsync(username);
            if (existed == null) throw new Exception("Not Found");
            if (existed.UserName != vm.UserName)
            {
            if (await _userManager.Users.AnyAsync(x => x.UserName == vm.UserName))
            {
                modelState.AddModelError("UserName", "This username is exis");
                return false;
            }

            }
            if (vm.ProfilePhoto != null)
            {
                if (!vm.ProfilePhoto.CheckType("image/"))
                {
                    modelState.AddModelError("Photo", "Your photo type is not true.Please use only image");
                    return false;
                }
                if (!vm.ProfilePhoto.ValidateSize(5 * 1024))
                {
                    modelState.AddModelError("Photo", "Your photo size max be 5mb");
                    return false;
                }
                string fileName = await vm.ProfilePhoto.CreateFileAsync(_env.WebRootPath, "assets", "img");
                existed.ProfileImage.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.ProfileImage = fileName;
            }
            existed.Name = vm.Name;
            existed.Surname=vm.Surname;
            existed.UserName=vm.UserName;
            var result =await _userManager.UpdateAsync(existed);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    modelState.AddModelError(string.Empty, item.Description);
                }
                return false;
            }
            return true;
        }
    }
}
