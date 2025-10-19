using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        public int? Popularity { get; set; }
        public int CategoryId { get; set; }
        public string? AppUserId { get; set; }
        public AppUser User { get; set; }
        public PaginateVm<Review>? ReviewVithPagination { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public Category? Category { get; set; }
        public ICollection<Meal> Meals { get; set; }

    }
}
