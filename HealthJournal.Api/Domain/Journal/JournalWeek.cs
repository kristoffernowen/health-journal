using HealthJournal.Api.Domain.Journal.Base;
using HealthJournal.Api.Domain.Journal.ValueObjects;

namespace HealthJournal.Api.Domain.Journal;

public class JournalWeek : EntityBase 
{
    public required WeekOfYear WeekOfYear { get; set; }
    public required DateOnly Start { get; set; }
    public required DateOnly End { get; set; }
    public string? Description { get; private set; }
    public List<JournalEntryBase> Entries { get; set; } = new List<JournalEntryBase>();
    public JournalUser JournalUser { get; set; } = null!;
    public Guid JournalUserId { get; set; }

    public static JournalWeek Create(WeekOfYear weekOfYear, string? description)
    {
        var (start, end) = weekOfYear.GetSpan();
        return new JournalWeek
        {
            WeekOfYear = weekOfYear,
            Start = start,
            End = end,
            Description = description
        };
    }

    public void AddEntry(JournalEntryBase entry)
    {
        if (!WeekOfYear.Contains(entry.Start) && !WeekOfYear.Contains(entry.End))
        {
            throw new ArgumentException("Entry date is not within the week.");
        }
        entry.JournalWeek = this;
        entry.JournalWeekId = Id;
        Entries.Add(entry);
    }
}