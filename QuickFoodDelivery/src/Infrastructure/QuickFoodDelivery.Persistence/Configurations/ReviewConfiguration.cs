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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(x => x.Description).IsRequired().HasMaxLength(750);
            builder.Property(x => x.PriceQuality).IsRequired();
            builder.Property(x => x.FoodQuality).IsRequired();
            builder.Property(x => x.Courtesy).IsRequired();
            builder.Property(x => x.Punctuality).IsRequired();
        }
    }
}
