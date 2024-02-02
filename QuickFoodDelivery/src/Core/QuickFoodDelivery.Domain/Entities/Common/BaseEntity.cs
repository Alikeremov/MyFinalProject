using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiredAt { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? ModifiedBy { get; set; }
        public BaseEntity()
        {
            CreatedBy = "Ali.Keremov";
            CreatedAt = DateTime.Now;
        }
    }
}
