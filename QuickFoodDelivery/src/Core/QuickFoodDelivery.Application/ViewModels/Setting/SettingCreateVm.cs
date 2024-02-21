using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class SettingCreateVm
    {
        [Required(ErrorMessage = "You can't enmpty this value")]
        public string Key { get; set; }
        [Required(ErrorMessage = "You can't enmpty this value")]
        public string Value { get; set; }
    }
}
