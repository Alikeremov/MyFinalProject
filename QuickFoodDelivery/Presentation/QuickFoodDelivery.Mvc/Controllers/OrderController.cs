using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Domain.Enums;
using QuickFoodDelivery.Persistence.DAL;
using System.Security.Claims;

namespace QuickFoodDelivery.Mvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAutenticationService _autenticationService;
        private readonly AppDbContext _context;

        public OrderController(IAutenticationService autenticationService, AppDbContext context)
        {
            _autenticationService = autenticationService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CheckOut()
        {
            OrderCreateVm orderVM = new OrderCreateVm();

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _autenticationService.GetUserAsync(User.Identity.Name);

                foreach (var item in user.BasketItems)
                {
                    Meal meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == item.MealId);
                    if (meal is not null)
                    {
                        orderVM.OrderItemVms.Add(new OrderItemVm
                        {
                            Price = meal.Price,
                            Count = item.Count,
                            MealId = meal.Id
                        });

                    }

                }
            }
            return View(orderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderCreateVm orderVM)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _autenticationService.GetUserAsync(User.Identity.Name);

                if (!ModelState.IsValid)
                {
                    foreach (var item in user.BasketItems)
                    {
                        Meal meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == item.MealId);
                        if (meal is not null)
                        {
                            orderVM.OrderItemVms.Add(new OrderItemVm
                            {
                                Price = meal.Price,
                                Count = item.Count,
                                MealId = meal.Id
                            });

                        }

                    }

                }


                Order order = new Order
                {
                    AppUserId = user.Id,
                    Status =OrderStatus.Pending,
                    Address = orderVM.UserAddress,
                    UserName = orderVM.UserName,
                    UserSurname = orderVM.UserSurname,
                    UserEmail = orderVM.UserEmail,
                    UserPhone=orderVM.UserPhoneNumber,
                    NoteForRestaurant=orderVM.NotesForRestaurant,
                    CreatedAt = DateTime.Now,
                    TotalPrice = 0,
                    OrderItems = new List<OrderItem>()
                };

                decimal total = 0;

                foreach (var item in user.BasketItems)
                {
                    Meal meal = await _context.Meals
                        .FirstOrDefaultAsync(x => x.Id == item.MealId);

                    total += item.Count * meal.Price;


                    if (meal is not null)
                    {
                        order.OrderItems.Add(new OrderItem
                        {
                            Count = item.Count,
                            Price = meal.Price,
                            MealId=meal.Id,
                            OrderId=order.Id
                        });
                    }
                }

                order.TotalPrice = total;
                await _context.Orders.AddAsync(order);
                user.BasketItems = new List<BasketItem>();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
