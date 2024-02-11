﻿using QuickFoodDelivery.Application.Abstractions.Repositories;
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
    public class FoodCategoryRepository : Repository<FoodCategory>, IFoodCategoryRepository
    {
        public FoodCategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}