using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalEntries;

public static class DeleteJournalEntryEndpoint
{
    public static RouteGroupBuilder MapDeleteJournalEntry(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", async (DataContext context, Guid id) =>
            {
                var fakeUser = FakeUserProvider.LoggedInDummy();
                var user = await context.JournalUsers
                    .Include(u => u.JournalWeeks)
                    .ThenInclude(jw => jw.Entries)
                    .FirstAsync(u => u.ExtUserId == fakeUser.ExtUserId);

                user.RemoveEntry(id);

                await context.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteJournalEntry");
        return group;
    }
}