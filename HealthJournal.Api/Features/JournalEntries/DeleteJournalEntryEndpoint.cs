namespace HealthJournal.Api.Features.JournalEntries;

public static class DeleteJournalEntryEndpoint
{
    public static RouteGroupBuilder MapDeleteJournalEntry(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", async (DataContext context, Guid id) =>
            {
                var journalEntry = await context.JournalEntries.FindAsync(id);
                if (journalEntry == null)
                {
                    return Results.NotFound();
                }
                context.JournalEntries.Remove(journalEntry);
                await context.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteJournalEntry");
        return group;
    }
}