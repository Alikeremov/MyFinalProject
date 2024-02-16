using QuickFoodDelivery.Application.Abstractions.Repositories;
using QuickFoodDelivery.Application.Abstractions.Services;
using QuickFoodDelivery.Domain.Entities;
using QuickFoodDelivery.Persistence.DAL;
using QuickFoodDelivery.Persistence.Implementations.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Implementations.Repositories
{
    public class CourierRepository : Repository<Courier>, ICourierRepository
    {
        public CourierRepository(AppDbContext context) : base(context)
        {
        }
    }
}
