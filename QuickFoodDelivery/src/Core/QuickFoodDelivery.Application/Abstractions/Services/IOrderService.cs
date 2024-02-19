using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using QuickFoodDelivery.Application.ViewModels;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<OrderCreateVm> CheckOuted(OrderCreateVm orderVM);
        Task<bool> CheckOut(OrderCreateVm orderVM, ModelStateDictionary modelstate, ITempDataDictionary keys, string stripeEmail, string stripeToken);
    }
}
