using HealthJournal.Api.Features.JournalEntries.Dtos;

namespace HealthJournal.Api.Features.JournalEntries;

public static class GetJournalEntryEndpoint
{
    public static RouteGroupBuilder MapGetJournalEntry(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (DataContext context, Guid id) =>
            {
                var journalEntry = await context.JournalEntries.FindAsync(id);
                if (journalEntry == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(new OutputJournalEntryDto(journalEntry.Id, journalEntry.Title,
                    journalEntry.Content, journalEntry.Date));
            })
            .WithName("GetJournalEntry");
        return group;
    }
}