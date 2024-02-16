using Microsoft.AspNetCore.Http;
using QuickFoodDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class CourierCreateVm
    {
        [Required]
        [MinLength(3)]
        [MaxLength(27)]
        public string Name { get; set; } = null!;
        [Required]
        [RegularExpression("^(?:+994|0)(d{2})[- ]?(d{3})[- ]?(d{2})[- ]?(d{2})$")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [MinLength(9, ErrorMessage = "Courier Email will be min 9 symbol")]
        [MaxLength(254, ErrorMessage = "Courier Email will be max 254 symbol")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$")]
        public string Email { get; set; } = null!;
        [Required]  
        public IFormFile Photo { get; set; } = null!;
        [Required]
        public CourierStatus CourierStatus { get; set; }
        public decimal Fee { get; set; }
        [Required]
        public string AppUserId { get; set; }
    }
}
