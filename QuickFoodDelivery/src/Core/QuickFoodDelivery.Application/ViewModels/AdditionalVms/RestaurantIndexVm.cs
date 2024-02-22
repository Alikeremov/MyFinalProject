using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels.AdditionalVms
{
	public class RestaurantIndexVm
	{
        public ICollection<CategoryItemVm> Categories { get; set; }
    }
}
