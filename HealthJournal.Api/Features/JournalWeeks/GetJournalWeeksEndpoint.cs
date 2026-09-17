using HealthJournal.Api.Domain.Journal.ValueObjects;
using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalWeeks
{
    public static class GetJournalWeeksEndpoint
    {
        public static RouteGroupBuilder MapGetJournalWeeks(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (DataContext context) =>
                {
                    var journalWeeks = await context.JournalWeeks
                        .Include(jw => jw.Entries)
                        .ToListAsync();
                    var output = journalWeeks.Select(jw => new OutputGetJournalWeekDto(
                        jw.Id,
                        jw.WeekOfYear,
                        jw.Entries.Select(e => new OutputGetJournalWeekEntryDto(
                            e.Id,
                            e.Title,
                            e.GetType().Name
                            )).ToList()
                    )).ToList();
                    return Results.Ok(output);
                })
                .WithName("GetJournalWeeks");
            return group;
        }
    }

    public record OutputGetJournalWeekDto(Guid Id, WeekOfYear WeekOfYear, List<OutputGetJournalWeekEntryDto> Entries);

    public record OutputGetJournalWeekEntryDto(Guid Id, string Title, string EntryType);
}
