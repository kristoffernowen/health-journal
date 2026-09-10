using HealthJournal.Api.Infrastructure.Data;

namespace HealthJournal.Api.Features.JournalEntries;

public static class UpdateJournalEntryEndpoint
{
    public static RouteGroupBuilder MapUpdateJournalEntry(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", async (DataContext context, Guid id, InputUpdateJournalEntryDto input) =>
            {
                //var user = FakeUserProvider.LoggedInDummy();
                var journalEntry = await context.JournalEntries.FindAsync(id);
                if (journalEntry == null)
                {
                    return Results.NotFound();
                }
                journalEntry.Title = input.Title;
                journalEntry.Description = input.Content;
                //journalEntry.JournalUserId = user.WeekAsString;
                await context.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateJournalEntry");
        return group;
    }
}

public record InputUpdateJournalEntryDto(string Title, string Content);