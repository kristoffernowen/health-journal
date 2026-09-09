using HealthJournal.Api.Domain.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthJournal.Api.Infrastructure.Data.Configurations
{
    public class JournalEntryBaseConfiguration : IEntityTypeConfiguration<JournalEntryBase>
    {
        public void Configure(EntityTypeBuilder<JournalEntryBase> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(1000).IsRequired();
            builder.HasDiscriminator<string>("EntryType")
                .HasValue<JournalEntryBase>("Base")
                .HasValue<ActivityEntry>("Activity");
            builder.HasIndex(e => e.JournalWeekId).HasDatabaseName("IX_JournalEntries_JournalWeekId");
        }
    }
}
