using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class ProfileUpdateVm
    {
        [Required]
        [MinLength(3)]
        [MaxLength(27)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ("You can only use letters in your Name"))]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(27)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ("You can only use letters in your SurName"))]
        public string Surname { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(27)]
        public string UserName { get; set; }
        [Required]
        public string ProfileImage { get; set; } = null!;
        public IFormFile? ProfilePhoto { get; set; }
    }
}
