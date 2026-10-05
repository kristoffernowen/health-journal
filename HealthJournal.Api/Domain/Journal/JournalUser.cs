namespace HealthJournal.Api.Domain.Journal;

public class JournalUser : EntityBase
{
    public string ExtUserId { get; set; } = string.Empty;
    public List<JournalWeek> JournalWeeks { get; set; } = new List<JournalWeek>();


    public void AddEntry(JournalEntryBase entry)
    {
        var entryWeek = WeekOfYear.FromDate(entry.Start);
        var week = GetOrCreateJournalWeek(entryWeek);
        week.AddEntry(entry);
    }

    public void UpdateEntry(Guid entryId, string? title, string? description, DateOnly? start, DateOnly? end)
    {
        var week = JournalWeeks.FirstOrDefault(jw => jw.Entries.Any(e => e.Id == entryId));
        if (week == null)
        {
            throw new EntryNotFoundException($"Week containing entry with ID {entryId} not found.");
        }
        week.UpdateEntry(entryId, title, description, start, end);
    }

    public void RemoveEntry(Guid entryId)
    {
        var week = JournalWeeks.FirstOrDefault(jw => jw.Entries.Any(e => e.Id == entryId));
        if (week == null)
        {
            throw new EntryNotFoundException($"Week containing entry with ID {entryId} not found.");
        }

        var entry = week.Entries.First(e => e.Id == entryId);
        
        week.Entries.Remove(entry);
    }

    public void UpdateWeekDescription(Guid id, string? description)
    {
        var week = JournalWeeks.FirstOrDefault(jw => jw.Id == id);
        if (week == null)
        {
            throw new EntryNotFoundException($"Week with ID {id} not found.");
        }
        week.UpdateDescription(description);
    }

    private JournalWeek GetOrCreateJournalWeek(WeekOfYear week)
    {
        var existing = JournalWeeks.FirstOrDefault(x => x.WeekOfYear == week);
        if (existing != null)
        {
            return existing;
        }
        var journalWeek = JournalWeek.Create(week, null);
        journalWeek.JournalUser = this;
        journalWeek.JournalUserId = Id;
        JournalWeeks.Add(journalWeek);
        return journalWeek;
    }
}
