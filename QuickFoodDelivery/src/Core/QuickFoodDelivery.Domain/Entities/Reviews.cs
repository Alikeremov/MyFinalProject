using QuickFoodDelivery.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Reviews:BaseEntity
    {
        public string Description { get; set; } = null!;
        public int UserId { get; set; }

    }
}
