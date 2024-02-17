﻿using QuickFoodDelivery.Domain.Entities.Common;
using QuickFoodDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Domain.Entities
{
    public class Order:BaseEntity
    {
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchasedAt { get; set; }
        //Relation properties
        public int? CourierId { get; set; }
        public Courier? Courier { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public List<BasketItem> BasketItems { get; set; }
    }
}