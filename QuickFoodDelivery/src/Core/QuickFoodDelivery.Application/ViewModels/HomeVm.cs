using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class HomeVm
    {
        public ICollection<RestaurantItemVm> RestaurantItems { get; set; }
        public ICollection<CategoryItemVm> CategoryItems { get; set; }
        public ICollection<EmploymentItemVm> Employments { get; set; }
    }
}
