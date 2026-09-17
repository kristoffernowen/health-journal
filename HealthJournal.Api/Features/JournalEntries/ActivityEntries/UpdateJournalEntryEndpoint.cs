using System.Diagnostics;
using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalEntries.ActivityEntries;

public static class UpdateActivityEntryEndpoint
{
    public static RouteGroupBuilder MapUpdateActivityEntry(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}", async (DataContext context, Guid id, InputUpdateActivityEntryDto input) =>
            {
                var fakeUser = FakeUserProvider.LoggedInDummy();
                var user = await context.JournalUsers
                    .Include(u => u.JournalWeeks)
                    .ThenInclude(jw => jw.Entries)
                    .FirstOrDefaultAsync(u => u.ExtUserId == fakeUser.ExtUserId);

                Debug.Assert(user != null, nameof(user) + " != null");
                user.UpdateEntry(id, input.Title, input.Description, input.PerformedAt, input.PerformedAt);

                context.JournalUsers.Update(user);
                await context.SaveChangesAsync();
                
                return Results.NoContent();
            })
            .WithName("UpdateJournalEntry");
        return group;
    }
}

public record InputUpdateActivityEntryDto(string? Title, string? Description, DateOnly? PerformedAt);