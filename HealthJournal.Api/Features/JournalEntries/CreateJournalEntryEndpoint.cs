using HealthJournal.Api.Models;

namespace HealthJournal.Api.Features.JournalEntries
{
    public static class CreateJournalEntryEndpoint
    {
        public static RouteGroupBuilder MapCreateJournalEntry(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (DataContext context, InputCreateJournalEntryDto input) =>
                {
                    var user = FakeUserProvider.LoggedInDummy();
                    var journalEntry = new JournalEntry(input.Title, input.Content, user.Id);
                    context.JournalEntries.Add(journalEntry);
                    await context.SaveChangesAsync();
                    return Results.Created($"/journal-entries/{journalEntry.Id}",
                        new OutputCreateJournalEntryDto(journalEntry.Id, journalEntry.Title, journalEntry.Content,
                            journalEntry.Date));
                })
                .WithName("CreateJournalEntry");
            return group;
        }
    }

    public record OutputCreateJournalEntryDto(Guid JournalEntryId, string JournalEntryTitle, string JournalEntryContent, DateTime JournalEntryDate);

    public record InputCreateJournalEntryDto(string Title, string Content);
}
