namespace HealthJournal.Api.Features.JournalEntries;

public static class DeleteJournalEntryEndpoint
{
    public static RouteGroupBuilder MapDeleteJournalEntry(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", async (
                IJournalEntryService journalEntryService,
                Guid id) =>
            {
                await journalEntryService.DeleteJournalEntryAsync(id);

                return Results.NoContent();
            })
            .RequireAuthorization("JournalWrite")
            .WithName("DeleteJournalEntry");
        return group;
    }
}
