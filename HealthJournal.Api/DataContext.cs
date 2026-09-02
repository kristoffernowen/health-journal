using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    }
}
