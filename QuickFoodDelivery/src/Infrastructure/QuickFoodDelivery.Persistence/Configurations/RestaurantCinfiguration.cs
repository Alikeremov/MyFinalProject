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
    public class RestaurantCinfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.Address).IsRequired().HasMaxLength(900);
            builder.Property(x => x.RestourantEmail).IsRequired().HasMaxLength(500);
            builder.HasIndex(x => x.RestourantEmail).IsUnique();
            builder.Property(x=>x.LocationCordinate).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.MinimumOrderAmount).IsRequired().HasColumnType("decimal(6,2)");
            builder.Property(x => x.Image).IsRequired();
        }
    }
}
