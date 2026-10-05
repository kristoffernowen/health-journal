namespace HealthJournal.Api.Features.JournalWeeks;

public interface IJournalWeekService
{
    Task<OutputGetJournalWeekByIdDto?> GetJournalWeekAsync(Guid id);
    Task<List<OutputGetJournalWeekDto>> GetAllJournalWeeksAsync();
    Task<bool> UpdateJournalWeekDescriptionAsync(Guid id, string? description);
}
