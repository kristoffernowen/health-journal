using HealthJournal.Api.Features.JournalEntries.Dtos;

namespace HealthJournal.Api.Features.JournalEntries;

public static class UpdateJournalEntryEndpoint
{
    public static RouteGroupBuilder MapUpdateJournalEntry(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", async (DataContext context, Guid id, InputJournalEntryDto input) =>
            {
                var journalEntry = await context.JournalEntries.FindAsync(id);
                if (journalEntry == null)
                {
                    return Results.NotFound();
                }
                journalEntry.Title = input.Title;
                journalEntry.Content = input.Content;
                await context.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateJournalEntry");
        return group;
    }
}