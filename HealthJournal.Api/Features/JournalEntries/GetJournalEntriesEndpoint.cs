using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalEntries;

public static class GetJournalEntriesEndpoint
{
    public static RouteGroupBuilder MapGetJournalEntries(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (DataContext context) =>
            {
                var user = FakeUserProvider.LoggedInDummy();
                var journalEntries = await context.JournalUsers.Include(u => u.JournalWeeks)
                    .ThenInclude(jw => jw.Entries)
                    .Where(u => u.Id == user.Id)
                    .SelectMany(u => u.JournalWeeks)
                    .SelectMany(jw => jw.Entries)
                    .ToListAsync();
                var output = journalEntries.Select(
                    j => new OutputGetJournalEntriesDto(j.Id, j.Title, j.Description, j.GetType().Name, j.CreatedAt));
                return Results.Ok(output);
            })
            .WithName("GetJournalEntries");
        return group;
    }
}

public record OutputGetJournalEntriesDto(Guid Id, string Title, string Description, string Type, DateTime CreatedAt
);