using HealthJournal.Api.Features.JournalEntries;
using HealthJournal.Api.Features.JournalEntries.ActivityEntries;
using HealthJournal.Api.Features.JournalWeeks;
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

var journalWeek = app.MapGroup("/journal-weeks")
    .WithTags("Journal Weeks")
    .WithGroupName("Journal Weeks")
    .WithDescription("Endpoints for managing journal weeks and journal entries in week views");

journalWeek.MapGetJournalWeeks();
journalWeek.MapGetJournalWeekById();
journalWeek.MapUpdateJournalWeekDescription();

var journalEntry = app.MapGroup("/journal-entries")
    .WithTags("Journal Entries")
    .WithGroupName("Journal Entries")
    .WithDescription("Endpoints for managing journal entries regardless of weeks");
// per type of entry, we can have different endpoints for creating and updating them, but the retrieval and deletion can be generic
journalEntry.MapCreateActivityEntry();
journalEntry.MapUpdateActivityEntry();

journalEntry.MapGetJournalEntry();
journalEntry.MapGetJournalEntries();
journalEntry.MapDeleteJournalEntry();

app.Run();