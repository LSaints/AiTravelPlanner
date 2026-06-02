using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Infrastructure.Persistence.Configurations;

public class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.ToTable("Trips");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Destination).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Country).IsRequired().HasMaxLength(100);
        builder.Property(t => t.StartDate).IsRequired();
        builder.Property(t => t.EndDate).IsRequired();
        builder.Property(t => t.NumberOfPeople).IsRequired();
        builder.Property(t => t.Budget).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(t => t.Objectives).IsRequired().HasMaxLength(2000);
        builder.Property(t => t.AdditionalNotes).HasMaxLength(4000);
        builder.Property(t => t.CreatedAt).IsRequired();
    }
}
