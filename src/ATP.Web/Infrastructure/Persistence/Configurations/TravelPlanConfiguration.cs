using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ATP.Web.Features.TravelPlans.Domain;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Infrastructure.Persistence.Configurations;

public class TravelPlanConfiguration : IEntityTypeConfiguration<TravelPlan>
{
    public void Configure(EntityTypeBuilder<TravelPlan> builder)
    {
        builder.ToTable("TravelPlans");

        builder.HasKey(tp => tp.Id);

        builder.Property(tp => tp.Prompt).IsRequired();
        builder.Property(tp => tp.GeneratedContent).IsRequired();
        builder.Property(tp => tp.PromptVersion).IsRequired().HasMaxLength(50);
        builder.Property(tp => tp.AiProvider).IsRequired().HasMaxLength(50);
        builder.Property(tp => tp.Model).IsRequired().HasMaxLength(50);
        builder.Property(tp => tp.TokensUsed);
        builder.Property(tp => tp.EstimatedCost).HasColumnType("decimal(18,2)");
        builder.Property(tp => tp.Status).IsRequired();
        builder.Property(tp => tp.GeneratedAt).IsRequired();

        builder.HasOne<Trip>()
            .WithMany()
            .HasForeignKey(tp => tp.TripId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
