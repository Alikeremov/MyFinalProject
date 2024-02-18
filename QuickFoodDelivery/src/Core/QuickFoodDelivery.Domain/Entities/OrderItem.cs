using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public decimal Price { get; set; }
        public decimal Count { get; set; }
        //Relation properties
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
