using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class OrderCreateVm
    {
        [Required]
        [MinLength(3)]
        [MaxLength(27)]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(29)]
        public string UserSurname { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string UserAddress { get; set; }
        [Required]
        [MinLength(9)]
        [MaxLength(254)]
        public string UserEmail { get; set; }
        [Required]
        [MinLength(9, ErrorMessage = "Courier Email will be min 9 symbol")]
        [MaxLength(254, ErrorMessage = "Courier Email will be max 254 symbol")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$")]
        public string UserPhoneNumber { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(500)]
        public string NotesForRestaurant { get; set; }
        public int CourierId { get; set; }
        public ICollection<string>? RestaurantAddreses { get; set; }
        public ICollection<OrderItemVm> OrderItemVms { get; set; }= new List<OrderItemVm>();
    }
}
