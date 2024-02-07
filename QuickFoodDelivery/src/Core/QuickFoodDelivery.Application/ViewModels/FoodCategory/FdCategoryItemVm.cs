using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class FdCategoryItemVm
    {
        public string Name { get; set; } = null!;
        public int Id { get; set; }
        public ICollection<Meal> Meals { get; set; }
    }
}
