using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class EmploymentItemVm
    {
        public int Id { get; set; }
        public string Tittle { get; set; } = null!;
        public string Subtittle { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; }=null!;
        public string ButtonText { get; set; } = null!;
    }
}
