using HealthJournal.Api;
using HealthJournal.Api.Features.JournalEntries;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddOpenApi();
var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Hello World!");

// app.MapJournalEntryEndpoints();
var journalEntry = app.MapGroup("/journalentries").WithTags("Journal Entries");
journalEntry.MapCreateJournalEntry();
journalEntry.MapGetJournalEntry();
journalEntry.MapGetJournalEntries();
journalEntry.MapUpdateJournalEntry();
journalEntry.MapDeleteJournalEntry();

app.Run();