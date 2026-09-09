using HealthJournal.Api.Domain.Journal.Base;

namespace HealthJournal.Api.Domain.Journal
{
    public abstract class JournalEntryBase : EntityBase
    {
        protected const int MaxTitleLength = 100;
        protected const int MaxContentLength = 1000;
        public abstract DateOnly Start { get; }
        public abstract DateOnly End { get; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public JournalWeek JournalWeek { get; set; } = null!;
        public Guid JournalWeekId { get; set; } 

        protected static string ValidateTitle(string title)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            if(title.Length > MaxTitleLength)
            {
                throw new ArgumentException($"Title cannot be longer than {MaxTitleLength} characters.");
            }
            return title;
        }

        protected static string ValidateDescription(string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description);
            if(description.Length > MaxContentLength)
            {
                throw new ArgumentException($"Description cannot be longer than {MaxContentLength} characters.");
            }
            return description;
        }
    }
}
