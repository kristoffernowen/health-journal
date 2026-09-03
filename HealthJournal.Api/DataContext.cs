using HealthJournal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
