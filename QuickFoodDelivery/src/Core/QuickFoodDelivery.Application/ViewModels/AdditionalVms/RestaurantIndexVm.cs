using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
	public class RestaurantIndexVm
	{
        public ICollection<CategoryItemVm> Categories { get; set; }
        public ICollection<RestaurantItemVm> Restaurants { get; set; }
        public int? CategoryId { get; set; }
        public string? SearchItem { get; set; }
        public int? Order { get; set; }
    }
}
