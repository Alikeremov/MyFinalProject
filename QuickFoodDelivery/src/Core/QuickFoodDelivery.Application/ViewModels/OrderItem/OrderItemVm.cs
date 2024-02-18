using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class OrderItemVm
    {
        public decimal Price { get; set; }
        public decimal Count { get; set; }
        public int MealId { get; set; }
    }
}
