using QuickFoodDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class OrderUpdateVm
    {
        public string? UserName { get; set; }
        public string? UserSurname { get; set; }
        public string? Address { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
