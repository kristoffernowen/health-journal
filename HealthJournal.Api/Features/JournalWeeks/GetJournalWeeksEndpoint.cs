namespace HealthJournal.Api.Features.JournalWeeks;

public static class GetJournalWeeksEndpoint
{
    public static RouteGroupBuilder MapGetJournalWeeks(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IJournalWeekService journalWeekService) =>
            {
                var journalWeeks = await journalWeekService.GetAllJournalWeeksAsync();
                var output = journalWeeks.Select(jw => jw.ToOutputGetJournalWeekDto()).ToList();
                
                return Results.Ok(output);
            })
            .WithName("GetJournalWeeks");
        return group;
    }
}

public record OutputGetJournalWeekDto(Guid Id, WeekOfYear WeekOfYear, List<OutputGetJournalWeekEntryDto> Entries);

public record OutputGetJournalWeekEntryDto(Guid Id, string Title, string EntryType);

public static class OutputGetJournalWeekDtoExtensions
{
    public static OutputGetJournalWeekDto ToOutputGetJournalWeekDto(this JournalWeek journalWeek)
    {
        return new OutputGetJournalWeekDto(
            journalWeek.Id,
            journalWeek.WeekOfYear,
            journalWeek.Entries.Select(e => new OutputGetJournalWeekEntryDto(
                e.Id,
                e.Title,
                e.GetType().Name
            )).ToList()
        );
    }
}
