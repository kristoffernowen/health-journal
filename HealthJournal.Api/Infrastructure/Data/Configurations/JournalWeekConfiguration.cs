using HealthJournal.Api.Domain.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthJournal.Api.Infrastructure.Data.Configurations
{
    public class JournalWeekConfiguration : IEntityTypeConfiguration<JournalWeek>
    {
        public void Configure(EntityTypeBuilder<JournalWeek> builder)
        {
            builder.HasKey(j => j.Id);
            builder.Property(j => j.Description).HasMaxLength(1000);
            builder.ComplexProperty(j => j.WeekOfYear); // Sic! Detta fungerar copilot - du har inte lärt dig det än bara

            builder.HasMany(j => j.Entries)
                .WithOne(e => e.JournalWeek)
                .HasForeignKey(e => e.JournalWeekId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(j => new { j.JournalUserId, j.Start }).HasDatabaseName("IX_JournalWeeks_JournalUserId_WeekOfYear");
        }
    }
}
