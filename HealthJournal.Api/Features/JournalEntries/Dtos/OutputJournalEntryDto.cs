namespace HealthJournal.Api.Features.JournalEntries.Dtos;

public record OutputJournalEntryDto(Guid Id, string Title, string Content, DateTime CreatedAt);