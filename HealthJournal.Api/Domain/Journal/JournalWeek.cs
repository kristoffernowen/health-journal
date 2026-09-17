using HealthJournal.Api.Domain.Journal.Base;
using HealthJournal.Api.Domain.Journal.ValueObjects;

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
            throw new ArgumentException("Entry date is not within the week.");
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
            throw new ArgumentException("Entry not found in the week.");
        }
        
        if (start.HasValue && !WeekOfYear.Contains(start.Value))
        {
            throw new ArgumentException("Start date is not within the week.");
        }
        if (end.HasValue && !WeekOfYear.Contains(end.Value))
        {
            throw new ArgumentException("End date is not within the week.");
        }

        entry.Update(title, description, start, end);
    }

    private static string ValidateDescription(string description)
    {
        return description.Length switch
        {
            < 3 => throw new ArgumentException("Description must be at least 3 characters long."),
            > 1000 => throw new ArgumentException("Description cannot be longer than 1000 characters."),
            _ => description
        };
    }
}