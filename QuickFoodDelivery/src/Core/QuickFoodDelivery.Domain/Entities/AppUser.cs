using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string ProfileImage { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public List<Order> Orders { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<Courier> Couriers { get; set; }
        public AppUser()
        {
            ProfileImage = "profile.png";
        }
    }
}
