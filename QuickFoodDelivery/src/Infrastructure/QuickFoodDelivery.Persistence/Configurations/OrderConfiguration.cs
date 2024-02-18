using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickFoodDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.UserEmail).IsRequired().HasMaxLength(254);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(30);
            builder.Property(x=>x.UserPhone).IsRequired().HasMaxLength(30);
            builder.Property(x=>x.UserSurname).IsRequired().HasMaxLength(30);
            builder.Property(x=>x.TotalPrice).IsRequired().HasColumnType("decimal(6,2)");
        }
    }
}
