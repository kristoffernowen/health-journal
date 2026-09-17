using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalWeeks
{
    public static class UpdateJournalWeekDescriptionEndpoint
    {
        public static RouteGroupBuilder MapUpdateJournalWeekDescription(this RouteGroupBuilder group)
        {
            group.MapPatch("/{id}", async (DataContext context, Guid id, UpdateJournalWeekDto input) =>
                {
                    var fakeUser = FakeUserProvider.LoggedInDummy();
                    var user = await context.JournalUsers
                        .Include(u => u.JournalWeeks)
                        .FirstAsync(u => u.ExtUserId == fakeUser.ExtUserId);

                    user.UpdateWeekDescription(id, input.Description);

                    context.JournalUsers.Update(user);
                    await context.SaveChangesAsync();
                    return Results.NoContent();
                })
                .WithName("UpdateJournalWeek")
                .Produces(StatusCodes.Status204NoContent);
            return group;
        }

        public record UpdateJournalWeekDto(string Description);
    }
}
