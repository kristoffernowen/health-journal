namespace HealthJournal.Api.Features.JournalEntries;

public static class GetJournalEntryEndpoint
{
    public static RouteGroupBuilder MapGetJournalEntry(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (IJournalEntryService journalEntryService, Guid id) =>
            {
                //needs user checking, but for now just return the entry if it exists
                var journalEntry = await journalEntryService.GetJournalEntryAsync(id);
                if (journalEntry == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(
                    journalEntry);
            })
            .RequireAuthorization("JournalRead")
            .WithName("GetJournalEntry");
        return group;
    }
}

public record OutputGetJournalEntryDto(Guid Id, string Title, string Description, DateOnly Start, DateOnly End, string Type, DateTime CreatedAt);

public static class OutputGetJournalEntryDtoExtensions
{
    public static OutputGetJournalEntryDto ToOutputGetJournalEntryDto(this JournalEntryBase journalEntry)
    {
        return new OutputGetJournalEntryDto(
            journalEntry.Id,
            journalEntry.Title,
            journalEntry.Description,
            journalEntry.Start,
            journalEntry.End,
            journalEntry.GetType().Name,
            journalEntry.CreatedAt);
    }
}
