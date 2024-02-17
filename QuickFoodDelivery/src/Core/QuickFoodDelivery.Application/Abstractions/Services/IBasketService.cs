using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IBasketService
    {
        Task<ICollection<BasketItemVm>> GetBasketItems();
        Task AddBasket(int id);
        Task Remove(int id);
        Task Minus(int id);
        Task Plus(int id);
    }
}
