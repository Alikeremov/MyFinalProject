using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
	public class CategoryItemVm
	{
		public string Name { get; set; } = null!;
        public int Id { get; set; }
        public ICollection<Restaurant> Restaurants { get; set; }
    }
}
