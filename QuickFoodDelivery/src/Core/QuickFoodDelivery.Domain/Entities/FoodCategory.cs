using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class FoodCategory:BaseEntityNameable
    {
        //Relational Properties
        public ICollection<Meal> Meals { get; set; }
    }
}
