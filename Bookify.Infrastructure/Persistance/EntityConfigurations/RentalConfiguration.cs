using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistance.EntityConfigurations;

internal class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(e =>e.StartDate).HasDefaultValueSql("CAST(GETDATE() AS Date)");


    }
}