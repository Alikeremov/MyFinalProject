using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Restaurant:BaseEntityNameable
    {
        public string Address { get; set; } = null!;
        public string CuisineType { get; set; } = null!;
        public string RestourantEmail { get; set; } = null!;
        public string? LocationCordinate { get; set; } = null!;
        public int Phone { get; set; }
        public DateTime? OpeningTime { get; set; }
        public DateTime? ClozedTime { get; set; }
        public bool IsOpening { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public string Image { get; set; } = null!;
        //Relational Properties
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int MenuId { get; set; }
        public Menu Menu { get; set; } = null!;
        public int ReviewId { get; set; }
        public Review Reviews { get; set; }
    }
}
