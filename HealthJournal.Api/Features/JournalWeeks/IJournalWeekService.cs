namespace HealthJournal.Api.Features.JournalWeeks;

public interface IJournalWeekService
{
    Task<JournalWeek?> GetJournalWeekAsync(Guid id);
    Task<List<JournalWeek>> GetAllJournalWeeksAsync();
    Task<bool> UpdateJournalWeekDescriptionAsync(Guid id, string? description);
}
