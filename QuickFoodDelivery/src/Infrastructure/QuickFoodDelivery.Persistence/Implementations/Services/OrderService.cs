﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Domain.Enums;
using Stripe;
using Stripe.Issuing;


namespace QuickFoodDelivery.Persistence.Implementations.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ICourierRepository _courierRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IAutenticationService _autentication;
        private readonly IMealRepository _mealRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IEmailService _service;

        public OrderService(IOrderRepository repository, ICourierRepository courierRepository, IHttpContextAccessor accessor, IAutenticationService autentication, IMealRepository mealRepository,IRestaurantRepository restaurantRepository,IEmailService service)
        {
            _repository = repository;
            _courierRepository = courierRepository;
            _accessor = accessor;
            _autentication = autentication;
            _mealRepository = mealRepository;
            _restaurantRepository = restaurantRepository;
            _service = service;
        }

        public async Task<OrderCreateVm> CheckOuted(OrderCreateVm orderVM)
        {
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _autentication.GetUserAsync(_accessor.HttpContext.User.Identity.Name);

                foreach (var item in user.BasketItems)
                {
                    Meal meal = await _mealRepository.GetByIdAsync(item.MealId, isDeleted: false);
                    if (meal is not null)
                    {
                        orderVM.OrderItemVms.Add(new OrderItemVm
                        {
                            Price = meal.Price,
                            Count = item.Count,
                            MealName = meal.Name,
                            MealId = meal.Id
                        });

                    }

                }
                ICollection<string> addresses = new List<string>();
                if (orderVM.OrderItemVms.Count != 0)
                {
                    foreach (var item in orderVM.OrderItemVms)
                    {
                        Meal meal = await _mealRepository.GetByIdAsync(item.MealId, isDeleted: false);
                        if (meal == null) throw new Exception("Meal not Found");
                        Restaurant restaurant = await _restaurantRepository.GetByIdAsync(meal.RestaurantId, isDeleted: false);
                        if (!addresses.Any(x => x == restaurant.Address))
                        {
                            addresses.Add(restaurant.Address);
                        }
                    }
                    orderVM.RestaurantAddreses=addresses;
                }
            }
            return orderVM;
        }
        public async Task<bool> CheckOut(OrderCreateVm orderVM, ModelStateDictionary modelstate, ITempDataDictionary tempdata, string stripeEmail, string stripeToken)
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
                    PurchasedAt= DateTime.Now,
                    TotalPrice = 0,
                    IsDeleted = false,
                    OrderItems = new List<OrderItem>()
                };
                decimal total = 0;
                foreach (var item in user.BasketItems)
                {
                    Meal meal = await _mealRepository.GetByIdAsync(item.MealId, isDeleted: false);

                    total += item.Count * meal.Price;
                    Restaurant restaurant =await _restaurantRepository.GetByIdnotDeletedAsync(meal.RestaurantId);
                   
                    if (meal is not null)
                    {
                        order.OrderItems.Add(new OrderItem
                        {
                            Count = item.Count,
                            Price = meal.Price,
                            MealName = meal.Name,
                            MealId = meal.Id,
                            OrderId = order.Id,
                            RestaurantId = restaurant.Id,
                        }) ;
                    }
                }
                ICollection<int> restaurantcount = new List<int>();
                if (order.OrderItems.Count != 0)
                {
                    for (int i = 0; i < order.OrderItems.Count; i++)
                    {
                        Meal meal = await _mealRepository.GetByIdAsync(order.OrderItems[i].MealId, isDeleted: false);
                        if (meal == null) throw new Exception("Meal not Found");
                        if (!restaurantcount.Any(x => x == meal.RestaurantId))
                        {
                            restaurantcount.Add(meal.RestaurantId);
                        }
                    }
                }
                total = restaurantcount.Count * 10 + total;
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
                    modelstate.AddModelError("Address", "You have problem on Payment");
                    return false;
                }


                order.TotalPrice = total;
                await _repository.AddAsync(order);
                user.BasketItems = new List<BasketItem>();
                await _repository.SaveChangesAsync();
                _accessor.HttpContext.Session.SetInt32("OrderId", order.Id);
            }
            return true;
        }
        public async Task<ICollection<OrderGetVm>> AcceptOrders(string userName)
        {
            AppUser user = await _autentication.GetUserAsync(userName);
            if (user == null) throw new Exception("Not Found");
            Courier courier = await _courierRepository.GetByExpressionAsync(x => x.AppUserId == user.Id, isDeleted: false);
            if (courier == null) throw new Exception("Not Found");
            ICollection<Order> orders = _repository.GetAllWhere(x => x.CourierId == courier.Id && x.Status != OrderStatus.Delivered, isDeleted: false, includes: new string[] { nameof(Order.OrderItems) }).ToList();
            return orders.Select(order => new OrderGetVm
            {
                UserName = order.UserName,
                UserAddress = order.Address,
                UserEmail = order.UserEmail,
                UserPhoneNumber = order.UserPhone,
                UserSurname = order.UserSurname,
                NotesForRestaurant = order.NoteForRestaurant,
                CourierId = (int)order.CourierId,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                Id = order.Id,
                OrderItemVms = order.OrderItems.Select(orderItem => new OrderItemVm
                {
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    MealId = orderItem.MealId,
                    MealName = orderItem.MealName,
                }).ToList(),

            }).ToList();
        }
        public async Task<ICollection<OrderGetVm>> DeliveredOrders(string userName)
        {
            AppUser user = await _autentication.GetUserAsync(userName);
            if (user == null) throw new Exception("Not Found");
            Courier courier = await _courierRepository.GetByExpressionAsync(x => x.AppUserId == user.Id, isDeleted: false);
            if (courier == null) throw new Exception("Not Found");
            ICollection<Order> orders = _repository.GetAllWhere(x => x.CourierId == courier.Id && x.Status == OrderStatus.Delivered, isDeleted: false, includes: new string[] { nameof(Order.OrderItems) }).ToList();
            return orders.Select(order => new OrderGetVm
            {
                UserName = order.UserName,
                UserAddress = order.Address,
                UserEmail = order.UserEmail,
                UserPhoneNumber = order.UserPhone,
                UserSurname = order.UserSurname,
                NotesForRestaurant = order.NoteForRestaurant,
                CourierId = (int)order.CourierId,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                Id = order.Id,
                OrderItemVms = order.OrderItems.Select(orderItem => new OrderItemVm
                {
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    MealId = orderItem.MealId,
                    MealName = orderItem.MealName,
                }).ToList(),

            }).ToList();
        }
        public async Task<ICollection<OrderGetVm>> GetAllOrdersByUserName(string username)
        {
            AppUser user = await _autentication.GetUserAsync(username);
            if (user == null) throw new Exception("Not Found");
            ICollection<Order> orders = _repository.GetAllWhere(x => x.AppUserId == user.Id, isDeleted: false, includes: new string[] { nameof(Order.OrderItems), nameof(Order.Courier) }).ToList();
            return orders.Select(order => new OrderGetVm
            {
                UserName = order.UserName,
                UserAddress = order.Address,
                UserEmail = order.UserEmail,
                UserPhoneNumber = order.UserPhone,
                UserSurname = order.UserSurname,
                NotesForRestaurant = order.NoteForRestaurant,
                CourierId = order.CourierId,
                Id = order.Id,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderItemVms = order.OrderItems.Select(orderItem => new OrderItemVm
                {
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    MealId = orderItem.MealId,
                    MealName = orderItem.MealName,
                }).ToList()
            }).ToList();
        }
        public async Task<ICollection<OrderGetVm>> GetAllDeliveredOrdersByUserName(string username)
        {
            AppUser user = await _autentication.GetUserAsync(username);
            if (user == null) throw new Exception("Not Found");
            ICollection<Order> orders = _repository.GetAllWhere(x => x.AppUserId == user.Id && x.Status==OrderStatus.Delivered, isDeleted: false, includes: new string[] { nameof(Order.OrderItems), nameof(Order.Courier) }).ToList();
            return orders.Select(order => new OrderGetVm
            {
                UserName = order.UserName,
                UserAddress = order.Address,
                UserEmail = order.UserEmail,
                UserPhoneNumber = order.UserPhone,
                UserSurname = order.UserSurname,
                NotesForRestaurant = order.NoteForRestaurant,
                CourierId = order.CourierId,
                Id = order.Id,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderItemVms = order.OrderItems.Select(orderItem => new OrderItemVm
                {
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    MealId = orderItem.MealId,
                    MealName = orderItem.MealName,
                }).ToList()
            }).ToList();
        }
        public async Task<OrderGetVm> GetOrderById(int id)
        {
            if (id < 1) throw new Exception("Bad Request");
            Order order = await _repository.GetByIdAsync(id, isDeleted: false, includes: new string[] { nameof(Order.OrderItems),nameof(Order.Courier) });
            if (order == null) throw new Exception("Not found");
            ICollection<string> addresses = new List<string>();
            if (order.OrderItems.Count != 0)
            {
                for (int i = 0; i < order.OrderItems.Count; i++)
                {
                    Meal meal = await _mealRepository.GetByIdAsync(order.OrderItems[i].MealId, isDeleted: false);
                    if (meal == null) throw new Exception("Meal not Found");
                    Restaurant restaurant=await _restaurantRepository.GetByIdAsync(meal.RestaurantId, isDeleted: false);
                    if (!addresses.Any(x=>x==restaurant.Address))
                    {
                        addresses.Add(restaurant.Address);
                    }
                }
            }
            Courier courier = new Courier();
            if (order.Courier != null)
            {
                 courier = order.Courier;
            }
            return new OrderGetVm
            {
                Id = order.Id,
                UserAddress=order.Address,
                UserName=order.UserName,
                UserSurname=order.UserSurname,
                UserEmail=order.UserEmail,
                UserPhoneNumber=order.UserPhone,
                NotesForRestaurant=order.NoteForRestaurant,
                Status=order.Status,
                RestaurantAddreses= addresses,
                TotalPrice=order.TotalPrice,
                CourierId=order.CourierId,
                Courier=new CourierItemVm
                {
                    Name=courier.Name,
                    Surname=courier.Surname,
                    Email=courier.Email,
                    Image=courier.Image,           
                },
                OrderItemVms= order.OrderItems.Select(orderItem => new OrderItemVm
                {
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    MealId = orderItem.MealId,
                    MealName = orderItem.MealName,
                }).ToList(),
            };

        }
        public async Task<OrderUpdateVm> Updated(int id, OrderUpdateVm vm)
        {
            if (id < 1) throw new Exception("Bad Request");
            Order existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            vm.OrderStatus = existed.Status;
            vm.UserSurname = existed.UserSurname;
            vm.UserName = existed.UserName;
            vm.Address = existed.Address;
            return vm;
        }
        public async Task<bool> Update(int id, OrderUpdateVm ordervm, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) return false;
            Order existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new Exception("Not Found");
            existed.Status = ordervm.OrderStatus;
            _repository.Update(existed);
            await _repository.SaveChangesAsync();
            if (existed.Status == OrderStatus.Delivered && existed.CourierId != null)
            {               
                Courier courier = await _courierRepository.GetByIdAsync((int)existed.CourierId, isDeleted: false, includes: new string[] { nameof(Courier.Orders) });
                if (courier.Orders.Count == 0)
                {
                    courier.Status = CourierStatus.Idle;
                    _courierRepository.Update(courier);
                    await _courierRepository.SaveChangesAsync();
                }
                
//                string link = _url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, HttpContext.Request.Scheme);
//                string body = $@"<div class=""card"" style=""width: 18rem;"">
//    <div class=""card-body"">
//        <h5 class=""card-title"">Hello Mr or Ms{existed.UserName}</h5>
//        <h6 class=""card-subtitle mb-2 text-body-secondary"">Your Order completed</h6>
//        <p class=""card-text"">Your</p>
//        <a href=""{link}"" class=""card-link"">Card link</a>
//        <a href=""#"" class=""card-link"">link</a>
//    </div>
//</div>";
//                await _service.SendEmailAsync(existed.UserEmail, "Order",body, true);
            }
            return true;
        }
        
    }
}
