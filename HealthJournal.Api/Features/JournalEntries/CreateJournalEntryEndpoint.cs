using HealthJournal.Api.Features.JournalEntries.Dtos;

namespace HealthJournal.Api.Features.JournalEntries
{
    public static class CreateJournalEntryEndpoint
    {
        public static RouteGroupBuilder MapCreateJournalEntry(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (DataContext context, InputJournalEntryDto input) =>
                {
                    var journalEntry = new JournalEntry(input.Title, input.Content);
                    context.JournalEntries.Add(journalEntry);
                    await context.SaveChangesAsync();
                    return Results.Created($"/journal-entries/{journalEntry.Id}",
                        new OutputJournalEntryDto(journalEntry.Id, journalEntry.Title, journalEntry.Content,
                            journalEntry.Date));
                })
                .WithName("CreateJournalEntry");
            return group;
        }
    }
}
