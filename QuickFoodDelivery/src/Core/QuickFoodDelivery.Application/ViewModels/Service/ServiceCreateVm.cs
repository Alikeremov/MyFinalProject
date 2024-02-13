using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class ServiceCreateVm
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Tittle { get; set; } = null!;
        [Required]
        [MinLength(5)]
        [MaxLength(300)]
        public string Subtittle { get; set; } = null!;
        [Required]
        [MinLength(2)]
        [MaxLength(150)]
        public string Icon { get; set; } = null!;
    }
}
