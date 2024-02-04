using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [MinLength(3)]
        [MaxLength(27)]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage =("You can only use letters in your Name")) ]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(27)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ("You can only use letters in your SurName"))]
        public string Surname { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string UserName { get; set; }=null!;
        [Required]
        [MinLength(9)]
        [MaxLength(254)]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = ("Your Email type is not true"))]
        public string Email { get; set; }=null!;
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ComfirmPassWord { get; set; } = null!;
        public IFormFile? ProfileImage { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public int Gender { get; set; }
    }
}
