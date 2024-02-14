using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class ForgotPasswordvm
    {
        [Required]
        [MinLength(9)]
        [MaxLength(254)]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = ("Your Email type is not true"))]
        public string Email { get; set; } = null!;
    }
}
