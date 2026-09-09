using HealthJournal.Api.Features.JournalEntries;
using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<DataContext>(opt => 
    opt.UseNpgsql(connectionString, npgsqlOptions => 
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 1,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging());
// remove sensitive data logging in production

builder.Services.AddOpenApi();
var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var journalEntry = app.MapGroup("/journalentries").WithTags("Journal Entries");
journalEntry.MapCreateJournalEntry();
journalEntry.MapGetJournalEntry();
journalEntry.MapGetJournalEntries();
journalEntry.MapUpdateJournalEntry();
journalEntry.MapDeleteJournalEntry();

app.Run();