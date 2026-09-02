namespace HealthJournal.Api.Features.JournalEntries.Dtos;

public record InputJournalEntryDto(string Title, string Content);
// Date? PerformedAt
// MyEnum TypeOfEntry: Week | Day | Activity
//If week: start-end date