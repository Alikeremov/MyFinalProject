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
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ("You can only use letters in your Name"))]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(29)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ("You can only use letters in your SurName"))]
        public string UserSurname { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string UserAddress { get; set; }
        [Required]
        [MinLength(9)]
        [MaxLength(254)]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = ("Your Email type is not true"))]
        public string UserEmail { get; set; }
        [Required]
        [RegularExpression("^(?:\\+994|0)(\\d{2})[- ]?(\\d{3})[- ]?(\\d{2})[- ]?(\\d{2})$")]
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
