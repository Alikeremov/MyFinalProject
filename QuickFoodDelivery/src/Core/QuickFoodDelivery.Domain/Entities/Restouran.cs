using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Restouran:BaseEntityNameable
    {
        public string Address { get; set; } = null!;
        public string CuisineType { get; set; } = null!;
        public string RestourantEmail { get; set; } = null!;
        public string? LocationCordinate { get; set; } = null!;
        public int Phone { get; set; }
        public DateTime? OpeningTime { get; set; }
        public bool OnlineOrderingStatus { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public string Image { get; set; } = null!;
    }
}
