using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Meal:BaseEntityNameable
    {
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int MenuId { get; set; }
        public decimal FinalReview { get; set; }
    }
}
