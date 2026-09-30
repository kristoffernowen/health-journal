namespace HealthJournal.Api;

public static class ProgramAppExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
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

        return app;
    }
}
