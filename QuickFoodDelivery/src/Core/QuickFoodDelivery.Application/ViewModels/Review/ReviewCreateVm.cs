using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class ReviewCreateVm
    {
        [Required]
        public int Quality { get; set; }
        [Required]
        [MaxLength(750)]
        public string Description { get; set; } = null!;
        [Required]
        public int RestaurantId { get; set; }


    }
}
