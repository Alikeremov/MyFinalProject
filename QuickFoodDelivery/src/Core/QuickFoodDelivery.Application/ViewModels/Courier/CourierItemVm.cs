using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class CourierItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Image { get; set; } = null!;
        public CourierStatus CourierStatus { get; set; }
        public decimal Fee { get; set; }
        public string AppUserId { get; set; }
    }
}
