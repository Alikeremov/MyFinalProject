﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Enums
{
    public enum OrderStatus
    {
        Pending=1,
        Preparing,
        OntheWay,
        Delivered
    }
}
