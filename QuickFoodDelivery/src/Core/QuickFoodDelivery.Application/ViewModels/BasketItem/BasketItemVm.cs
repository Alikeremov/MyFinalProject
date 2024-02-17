using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class BasketItemVm
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        //Relation properties
        public int MealId { get; set; }
        public string? AppUserId { get; set; }
        public int? OrderId { get; set; }
    }
}
