using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Template1.API.Infrastructure.EntityConfigurations
{
    using Model;
    class Template1EntityTypeConfiguration
        : IEntityTypeConfiguration<Template1>
    {
        public void Configure(EntityTypeBuilder<Template1> builder)
        {
            builder.ToTable("Template1");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();
        }
    }
}
