using HealthJournal.Api.Domain.Journal;
using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalEntries
{
    public static class CreateJournalEntryEndpoint
    {
        public static RouteGroupBuilder MapCreateJournalEntry(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (DataContext context, InputCreateJournalEntryDto input) =>
                {
                    var fakeUser = FakeUserProvider.LoggedInDummy();
                    var user = await context.JournalUsers
                        .Include(u => u.JournalWeeks)
                            .ThenInclude(jw => jw.Entries)
                        .FirstOrDefaultAsync(u => u.Id == fakeUser.Id);

                    if (user == null)
                    {
                        return Results.NotFound();
                    }

                    var journalEntry = ActivityEntry.Create(input.Title, input.Description, input.PerformedAt);
                    user.AddEntry(journalEntry);
                    context.ActivityEntries.Add(journalEntry);

                    await context.SaveChangesAsync();

                    return Results.Created($"/journal-entries/{journalEntry.Id}",
                        new OutputCreateJournalEntryDto(journalEntry.Id, journalEntry.Title, journalEntry.Description,
                            journalEntry.CreatedAt, journalEntry.PerformedAt));
                })
                .WithName("CreateJournalEntry");
            return group;
        }
    }

    public record OutputCreateJournalEntryDto(Guid Id, string Title, string Description, DateTime CreatedAt, DateOnly PerformedAt);

    public record InputCreateJournalEntryDto(string Title, string Description, DateOnly PerformedAt);
}
