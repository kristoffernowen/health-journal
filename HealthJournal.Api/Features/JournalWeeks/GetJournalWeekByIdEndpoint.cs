namespace HealthJournal.Api.Features.JournalWeeks;

public static class GetJournalWeekByIdEndpoint
{
    public static RouteGroupBuilder MapGetJournalWeekById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (Guid id, IJournalWeekService journalWeekService) =>
        {
            var journalWeek = await journalWeekService.GetJournalWeekAsync(id);
            if (journalWeek == null)
            {
                return Results.NotFound();
            }
            var output = journalWeek.ToOutputGetJournalWeekByIdDto();
            return Results.Ok(output);
        })
        .WithName("GetJournalWeekById");
        return group;
    }
}

public record OutputGetJournalWeekByIdDto(Guid Id, WeekOfYear WeekOfYear, DateOnly Start, DateOnly End, string? Description, List<OutputGetJournalWeekByIdEntryDto> Entries);
public record OutputGetJournalWeekByIdEntryDto(Guid Id, string Title, DateOnly Date, string EntryType);

public static class OutputGetJournalWeekByIdDtoExtensions
{
    public static OutputGetJournalWeekByIdDto ToOutputGetJournalWeekByIdDto(this JournalWeek journalWeek)
    {
        return new OutputGetJournalWeekByIdDto(
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
    }
}
