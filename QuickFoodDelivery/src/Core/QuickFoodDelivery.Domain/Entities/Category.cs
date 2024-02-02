using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Category:BaseEntityNameable
    {
        //Relational Properties
        public ICollection<Restaurant> Restaurants { get; set; }
    }
}
