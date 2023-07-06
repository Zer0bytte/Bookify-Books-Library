using Bookify.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistance.EntityConfigurations;

internal class RentalCopyConfiguration : IEntityTypeConfiguration<RentalCopy>
{
    public void Configure(EntityTypeBuilder<RentalCopy> builder)
    {
        builder.Property(e => e.RentalDate).HasDefaultValue(DateTime.Today);
        builder.HasKey(e => new { e.RentalId, e.BookCopyId });
        builder.HasQueryFilter(e =>!e.Rental!.IsDeleted);
    }
}
