using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class AboutVm
    {
        public RestaurantItemVm Restaurant { get; set; }
        public ReviewCreateVm ReviewCreateVm { get; set; } = new ReviewCreateVm();
    }
}
