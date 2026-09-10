using HealthJournal.Api.Domain.Journal.Base;
using HealthJournal.Api.Domain.Journal.ValueObjects;

namespace HealthJournal.Api.Domain.Journal
{
    public class JournalUser : EntityBase
    {
        public string ExtUserId { get; set; } = string.Empty;
        public List<JournalWeek> JournalWeeks { get; set; } = new List<JournalWeek>();

        public JournalWeek GetOrCreateJournalWeek(WeekOfYear week)
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

        public void AddEntry(JournalEntryBase entry)
        {
            var entryWeek = WeekOfYear.FromDate(entry.Start);
            var week = GetOrCreateJournalWeek(entryWeek);
            week.AddEntry(entry);
        }
    }
}
