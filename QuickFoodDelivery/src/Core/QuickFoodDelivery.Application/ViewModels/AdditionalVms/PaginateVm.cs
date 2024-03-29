﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Application.ViewModels
{
    public class PaginateVm<T>where T : class,new()
    {
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public ICollection<T> Items { get; set; }
    }
}
