﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class RestaurantAdminVm
    {
        public MealCreateVm? CreateMealVm { get; set; }
        public RestaurantUpdateVm? RestaurantUpdateVm { get; set; }
    }
}