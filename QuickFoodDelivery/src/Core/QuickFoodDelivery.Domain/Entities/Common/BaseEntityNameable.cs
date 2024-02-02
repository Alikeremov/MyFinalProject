using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities.Common
{
    public abstract class BaseEntityNameable:BaseEntity
    {
        public string Name { get; set; } = null!;
    }
}
