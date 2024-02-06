using Microsoft.AspNetCore.Http;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class RestaurantCreateVm
    {
        [Required(ErrorMessage ="Restauran name was required")]
        [MinLength(3,ErrorMessage ="RestauranName contain min 3 symbol")]
        [MaxLength(120,ErrorMessage ="RestauranName contain max 120 symbol")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage ="Address was required")]
        [MinLength(5,ErrorMessage ="Restauran Address will min 5 symbol")]
        [MaxLength(150,ErrorMessage ="Restoran Address will be max 150 symbol")]
        public string Address { get; set; } = null!;
        [Required]
        [MinLength(9,ErrorMessage ="Restouran Email will be min 9 symbol")]
        [MaxLength(254,ErrorMessage ="Resyouran Email will be max 254 symbol")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$")]
        public string RestourantEmail { get; set; } = null!;
        [MaxLength(50,ErrorMessage ="Location Cordinate will be maximum 50 symbol")]
        [MinLength(4,ErrorMessage ="Location Cordinate will be min 4 symbol")]
        public string? LocationCordinate { get; set; } = null!;
        [Required(ErrorMessage ="Your Phone was Required")]
        [MinLength(9,ErrorMessage = "Your Phone must be min 9 symbol")]
        [MaxLength(15,ErrorMessage ="Your Phone must be max 15 symbol")]
        public int Phone { get; set; }
        public DateTime? OpeningTime { get; set; }
        public DateTime? ClozedTime { get; set; }
        public bool IsOpening { get; set; }
        [Required(ErrorMessage = "Your MinimumOrderAmount was Reqiured")]
        public decimal MinimumOrderAmount { get; set; }
        [Required(ErrorMessage = "Your Category was Reqiured")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Your Photo was Reqiured")]
        public IFormFile Photo { get; set; } = null!;
        public ICollection<Category> Categories { get; set; }
    }
}
