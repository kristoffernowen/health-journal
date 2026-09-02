using HealthJournal.Api.Features.JournalEntries.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalEntries;

public static class GetJournalEntriesEndpoint
{
    public static RouteGroupBuilder MapGetJournalEntries(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (DataContext context) =>
            {
                var journalEntries = await context.JournalEntries.ToListAsync();
                var output = journalEntries.Select(j => new OutputJournalEntryDto(j.Id, j.Title, j.Content, j.Date));
                return Results.Ok(output);
            })
            .WithName("GetJournalEntries");
        return group;
    }
}