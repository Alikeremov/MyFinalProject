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
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        public async Task<ICollection<OrderItemVm>> GetAllByRestaurantId(int restaurantId)
        {
            ICollection<OrderItem> orderItems = await _orderItemRepository.GetAllWhere(x => x.RestaurantId == restaurantId,isDeleted:null).ToListAsync();
            return orderItems.Select(orderItem => new OrderItemVm
            {
                Price = orderItem.Price,
                Count = orderItem.Count,
                MealId= orderItem.MealId,
                MealName=orderItem.MealName,
                RestaurantId=orderItem.RestaurantId
            }).ToList();
        }
    }
}
