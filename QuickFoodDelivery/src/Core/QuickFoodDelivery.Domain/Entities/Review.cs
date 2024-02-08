using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Review:BaseEntity
    {
        public string Description { get; set; } = null!;
        public int FoodQuality { get; set; }
        public int PriceQuality { get; set; }
        public int Punctuality { get; set; }
        public int Courtesy { get; set; }
        //Reletional properties
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

    }
}
