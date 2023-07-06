using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistance.EntityConfigurations;

internal class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.HasIndex(e => new { e.Name, e.GovernorateId }).IsUnique();

        builder.Property(e => e.Name).HasMaxLength(100);
    }
}