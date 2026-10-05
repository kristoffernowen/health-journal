namespace HealthJournal.Api.Features.JournalEntries.Service;

public interface IJournalEntryService
{
    Task<List<OutputGetJournalEntriesDto>> GetJournalEntriesAsync();
    Task<OutputGetJournalEntryDto?> GetJournalEntryAsync(Guid id);
    Task DeleteJournalEntryAsync(Guid id);
    Task<OutputCreateActivityEntryDto> CreateJournalEntryAsync(InputCreateActivityEntryDto entry);
    Task UpdateJournalEntryAsync(Guid id, InputUpdateActivityEntryDto entry);
}
