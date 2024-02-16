using QuickFoodDelivery.Domain.Entities.Common;
using QuickFoodDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Courier:BaseEntityNameable
    {
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Fee { get; set; }
        public CourierStatus Status { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public Courier()
        {
            Image= "profile.png";
            Status = CourierStatus.Idle;
        }
    }
}
