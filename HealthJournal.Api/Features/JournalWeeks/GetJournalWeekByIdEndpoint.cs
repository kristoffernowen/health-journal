using HealthJournal.Api.Domain.Journal;
using HealthJournal.Api.Domain.Journal.ValueObjects;
using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalWeeks
{
    public static class GetJournalWeekByIdEndpoint
    {
        public static RouteGroupBuilder MapGetJournalWeekById(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:guid}", async (Guid id, DataContext context) =>
                {
                    var journalWeek = await context.JournalWeeks
                        .Include(jw => jw.Entries)
                        .FirstOrDefaultAsync(jw => jw.Id == id);
                    if (journalWeek == null)
                    {
                        return Results.NotFound();
                    }
                    var output = new OutputGetJournalWeekByIdDto(
                        journalWeek.Id,
                        journalWeek.WeekOfYear,
                        journalWeek.Start,
                        journalWeek.End,
                        journalWeek.Description,
                        journalWeek.Entries.Select(e => new OutputGetJournalWeekByIdEntryDto(
                            e.Id,
                            e.Title,
                            e.Start,
                            e switch
                            {
                                ActivityEntry => "Activity",
                                _ => "Unknown"
                            }
                        ))
                        .OrderBy(e => e.Date)
                        .ToList()
                    );
                    return Results.Ok(output);
                })
                .WithName("GetJournalWeekById");
            return group;
        }
    }

    public record OutputGetJournalWeekByIdDto(Guid Id, WeekOfYear WeekOfYear, DateOnly Start, DateOnly End, string? Description, List<OutputGetJournalWeekByIdEntryDto> Entries);
    public record OutputGetJournalWeekByIdEntryDto(Guid Id, string Title, DateOnly Date, string EntryType);
}
