namespace HealthJournal.Api.Domain.Journal;

public class JournalWeek : EntityBase
{
    public required WeekOfYear WeekOfYear { get; set; }
    public required DateOnly Start { get; set; }
    public required DateOnly End { get; set; }
    public string? Description { get; set; }
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
            Description = description != null ? ValidateDescription(description) : null
        };
    }

    public void UpdateDescription(string? description)
    {
        Description = description != null ? ValidateDescription(description) : throw new ArgumentException("Description cannot be null when you update.");
    }

    public void AddEntry(JournalEntryBase entry)
    {
        if (!WeekOfYear.Contains(entry.Start) && !WeekOfYear.Contains(entry.End))
        {
            throw new EntryOutsideWeekException(entry.Start, entry.End, WeekOfYear);
        }
        entry.JournalWeek = this;
        entry.JournalWeekId = Id;
        Entries.Add(entry);
    }

    public void UpdateEntry(Guid entryId, string? title, string? description, DateOnly? start, DateOnly? end)
    {
        var entry = Entries.FirstOrDefault(e => e.Id == entryId);
        if (entry == null)
        {
            throw new EntryNotFoundException($"Entry with ID {entryId} not found in week.");
        }

        if (start.HasValue && !WeekOfYear.Contains(start.Value))
        {
            throw new EntryOutsideWeekException(start.Value, end ?? entry.End, WeekOfYear); //should work but may produce funny message
        }
        if (end.HasValue && !WeekOfYear.Contains(end.Value))
        {
            throw new EntryOutsideWeekException(start ?? entry.Start, end.Value, WeekOfYear); //should work but may produce funny message
        }

        entry.Update(title, description, start, end);
    }

    private static string ValidateDescription(string description)
    {
        return description.Length switch
        {
            < 3 => throw new DescriptionTooShortException("Description must be at least 3 characters long."),
            > 1000 => throw new DescriptionTooLongException("Description cannot be longer than 1000 characters."),
            _ => description
        };
    }
}
