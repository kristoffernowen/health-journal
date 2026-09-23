// Global usings consolidated in GlobalUsings.cs
namespace HealthJournal.Api.Features.JournalEntries;

public static class DeleteJournalEntryEndpoint
{
    public static RouteGroupBuilder MapDeleteJournalEntry(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", async (
                DataContext context, 
                ILoggerFactory loggerFactory, 
                Guid id) =>
            {
                var logger = loggerFactory.CreateLogger("DeleteJournalEntryEndpoint");
                var fakeUser = FakeUserProvider.LoggedInDummy();
                var user = await context.JournalUsers
                    .Include(u => u.JournalWeeks)
                    .ThenInclude(jw => jw.Entries)
                    .FirstAsync(u => u.ExtUserId == fakeUser.ExtUserId);

                user.RemoveEntry(id);

                await context.SaveChangesAsync();

                logger.LogInformation("Deleted activity entry with ID {Id}", id);
                
                return Results.NoContent();
            })
            .WithName("DeleteJournalEntry");
        return group;
    }
}