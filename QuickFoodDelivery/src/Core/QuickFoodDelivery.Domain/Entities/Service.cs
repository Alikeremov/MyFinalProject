using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Service:BaseEntity
    {
        public string Tittle { get; set; } = null!;
        public string Subtittle { get; set; } = null!;
        public string Icon { get; set; } = null!;
    }
}
