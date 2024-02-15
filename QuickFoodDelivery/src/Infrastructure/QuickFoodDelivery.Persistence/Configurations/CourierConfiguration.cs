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
    public class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(27);
            builder.Property(x=>x.Fee).IsRequired().HasColumnType("decimal(6,2)");
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(254);
        }
    }
}
