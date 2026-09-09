using HealthJournal.Api.Domain.Journal;
using HealthJournal.Api.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Infrastructure.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<JournalWeek> JournalWeeks { get; set; } = null!;
        public DbSet<JournalEntryBase> JournalEntries { get; set; } = null!;
        public DbSet<ActivityEntry> ActivityEntries { get; set; } = null!;
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new JournalWeekConfiguration());
            modelBuilder.ApplyConfiguration(new JournalEntryBaseConfiguration());
        }
    }
}
