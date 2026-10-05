namespace HealthJournal.Api.Features.JournalEntries;

public static class GetJournalEntriesEndpoint
{
    public static RouteGroupBuilder MapGetJournalEntries(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IJournalEntryService journalEntryService) =>
            {
                var journalEntries = await journalEntryService.GetJournalEntriesAsync();
                return Results.Ok(journalEntries);
            })
            .WithName("GetJournalEntries");
        return group;
    }
}

public record OutputGetJournalEntriesDto(Guid Id, string Title, string Description, string Type, DateTime CreatedAt
);
