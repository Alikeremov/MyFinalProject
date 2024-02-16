using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class BasketItem:BaseEntity
    {
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
