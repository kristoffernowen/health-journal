using HealthJournal.Api.Infrastructure.Data;

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

                return Results.Ok(
                    new OutputGetJournalEntryDto(
                        journalEntry.Id, 
                        journalEntry.Title,
                        journalEntry.Description, 
                        journalEntry.Start, 
                        journalEntry.End, 
                        journalEntry.GetType().Name, 
                        journalEntry.CreatedAt));
            })
            .WithName("GetJournalEntry");
        return group;
    }
}

public record OutputGetJournalEntryDto(Guid Id, string Title, string Description, DateOnly Start, DateOnly End, string Type, DateTime CreatedAt);