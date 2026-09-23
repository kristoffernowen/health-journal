// Global usings consolidated in GlobalUsings.cs
namespace HealthJournal.Api.Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<JournalUser> JournalUsers { get; set; } = null!;
    public DbSet<JournalWeek> JournalWeeks { get; set; } = null!;
    public DbSet<JournalEntryBase> JournalEntries { get; set; } = null!;
    public DbSet<ActivityEntry> ActivityEntries { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
