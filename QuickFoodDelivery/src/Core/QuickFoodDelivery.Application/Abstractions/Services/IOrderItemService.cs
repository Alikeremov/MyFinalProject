using QuickFoodDelivery.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IOrderItemService
    {
        Task<ICollection<OrderItemVm>> GetAllByRestaurantId(int restaurantId);
    }
}
