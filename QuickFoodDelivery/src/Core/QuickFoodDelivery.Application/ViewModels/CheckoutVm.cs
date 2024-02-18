using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class CheckoutVm
    {
        public ICollection<BasketItemVm>? BasketItems { get; set; }
        public OrderCreateVm? OrderCreateVm { get; set; }
    }
}
