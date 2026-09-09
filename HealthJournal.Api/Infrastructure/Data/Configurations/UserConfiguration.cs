using HealthJournal.Api.Domain.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthJournal.Api.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.ExtUserId).HasMaxLength(50).IsRequired();
            builder.HasMany(u => u.JournalWeeks)
                .WithOne(jw => jw.User)
                .HasForeignKey(jw => jw.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // seed fakeuser for now - use fakeuser for testing purposes
            builder.HasData(
                new User
                {
                    Id = Guid.Parse("6c23bc95-9c5f-4ff6-888c-3b2eccf766f2"),
                    ExtUserId = "dummy-user",
                    CreatedAt = new DateTime(2026, 09, 09, 12, 10, 10, DateTimeKind.Utc)
                });
        }
    }
}
