using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class EmploymentCreateVm
    {
        [Required]
        [MinLength(5)]
        [MaxLength(70)]
        public string Tittle { get; set; } = null!;
        [Required]
        [MinLength(15)]
        [MaxLength(150)]
        public string Subtittle { get; set; } = null!;
        [Required]
        [MinLength(20)]
        [MaxLength(300)]
        public string Description { get; set; } = null!;
        [Required]
        [MinLength(2)]
        [MaxLength(60)]
        public string ControllerName { get; set; } = null!;
        [Required]
        [MinLength(2)]
        [MaxLength(60)]
        public string ActionName { get; set; } = null!;
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string ButtonText { get; set; } = null!;
    }
}
