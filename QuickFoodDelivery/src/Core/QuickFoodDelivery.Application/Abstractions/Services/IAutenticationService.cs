using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IAutenticationService
    {
        Task<List<string>> Register(RegisterVm vm);   
        Task<List<string>> Login(LoginVm vm);
        Task Logout();
        Task CreateRoleAsync();
    }
}
