using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class OrderGetVm
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserAddress { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string NotesForRestaurant { get; set; }
        public int CourierId { get; set; }
        public ICollection<string>? RestaurantAddreses { get; set; }
        public ICollection<OrderItemVm> OrderItemVms { get; set; } = new List<OrderItemVm>();
    }
}
