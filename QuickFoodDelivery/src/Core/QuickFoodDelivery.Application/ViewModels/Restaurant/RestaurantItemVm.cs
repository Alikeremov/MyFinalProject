using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class RestaurantItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string RestourantEmail { get; set; } = null!;
        public string? LocationCordinate { get; set; } = null!;
        public int Phone { get; set; }
        public DateTime? OpeningTime { get; set; }
        public DateTime? ClozedTime { get; set; }
        public bool IsOpening { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public string Image { get; set; } = null!;
        public int CategoryId { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Meal> Meals { get; set; }

    }
}
