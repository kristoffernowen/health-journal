using HealthJournal.Api.Domain.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthJournal.Api.Infrastructure.Data.Configurations
{
    public class JournalUserConfiguration : IEntityTypeConfiguration<JournalUser>
    {
        public void Configure(EntityTypeBuilder<JournalUser> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.ExtUserId).HasMaxLength(50).IsRequired();
            builder.HasMany(u => u.JournalWeeks)
                .WithOne(jw => jw.JournalUser)
                .HasForeignKey(jw => jw.JournalUserId)
                .OnDelete(DeleteBehavior.Cascade);
            // seed fakeuser for now - use fakeuser for testing purposes
            builder.HasData(
                new JournalUser
                {
                    Id = Guid.Parse("6c23bc95-9c5f-4ff6-888c-3b2eccf766f2"),
                    ExtUserId = "dummy-user",
                    CreatedAt = new DateTime(2026, 09, 09, 12, 10, 10, DateTimeKind.Utc)
                });
        }
    }
}
