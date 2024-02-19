using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Domain.Enums;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _repository;
		private readonly ICourierRepository _courierRepository;
		private readonly IHttpContextAccessor _accessor;
        private readonly IAutenticationService _autentication;
        private readonly IMealRepository _mealRepository;

        public OrderService(IOrderRepository repository,ICourierRepository courierRepository,IHttpContextAccessor accessor,IAutenticationService autentication,IMealRepository mealRepository)
        {
            _repository = repository;
			_courierRepository = courierRepository;
			_accessor = accessor;
            _autentication = autentication;
            _mealRepository = mealRepository;
        }
        
		public async Task<OrderCreateVm> CheckOuted(OrderCreateVm orderVM)
        {
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);

                foreach (var item in user.BasketItems)
                {
                    Meal meal = await _mealRepository.GetByIdAsync(item.MealId,isDeleted:false);
                    if (meal is not null)
                    {
                        orderVM.OrderItemVms.Add(new OrderItemVm
                        {
                            Price = meal.Price,
                            Count = item.Count,
                            MealName=meal.Name,
                            MealId = meal.Id
                        });

                    }

                }
            }
            return orderVM;
        }
        public async Task<bool> CheckOut(OrderCreateVm orderVM,ModelStateDictionary modelstate,ITempDataDictionary tempdata,string stripeEmail,string stripeToken)
        {
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);

                if (!modelstate.IsValid)
                {
                    return false;

                }

                Order order = new Order
                {
                    AppUserId = user.Id,
                    Status = OrderStatus.Pending,
                    Address = orderVM.UserAddress,
                    UserName = orderVM.UserName,
                    UserSurname = orderVM.UserSurname,
                    UserEmail = orderVM.UserEmail,
                    UserPhone = orderVM.UserPhoneNumber,
                    NoteForRestaurant = orderVM.NotesForRestaurant,
                    CreatedAt = DateTime.Now,
                    TotalPrice = 0,
                    IsDeleted = false,
                    OrderItems = new List<OrderItem>()
                };
                decimal total = 0;
                foreach (var item in user.BasketItems)
                {
                    Meal meal = await _mealRepository.GetByIdAsync(item.MealId, isDeleted: false);

                    total += item.Count * meal.Price;


                    if (meal is not null)
                    {
                        order.OrderItems.Add(new OrderItem
                        {
                            Count = item.Count,
                            Price = meal.Price,
                            MealName=meal.Name,
                            MealId = meal.Id,
                            OrderId = order.Id
                        });
                    }
                }
                var optionCust = new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Name = user.Name + " " + user.Surname,
                    Phone = order.UserPhone
                };
                var serviceCust = new CustomerService();
                Customer customer = serviceCust.Create(optionCust);

                total = total * 100;
                var optionsCharge = new ChargeCreateOptions
                {

                    Amount = (long)total,
                    Currency = "USD",
                    Description = "Product Selling amount",
                    Source = stripeToken,
                    ReceiptEmail = stripeEmail


                };
                var serviceCharge = new ChargeService();
                Charge charge = serviceCharge.Create(optionsCharge);

                if (charge.Status != "succeeded")
                {
                    modelstate.AddModelError("Address", "Odenishde problem var");
                    return false;
                }


                order.TotalPrice = total;
                await _repository.AddAsync(order);
                user.BasketItems = new List<BasketItem>();
                await _repository.SaveChangesAsync();
                _accessor.HttpContext.Session.SetInt32("OrderId", order.Id);
                //tempdata["OrderId"]=order.Id;
            }
                return true;
        }
    }
}
