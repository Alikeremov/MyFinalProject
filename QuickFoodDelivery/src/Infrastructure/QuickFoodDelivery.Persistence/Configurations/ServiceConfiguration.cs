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
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(x => x.Subtittle).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Tittle).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Icon).IsRequired().HasMaxLength(150);
        }
    }
}
