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
            return title.Length switch
            {
                < 3 => throw new ArgumentException("Title must be at least 3 characters long."),
                > MaxTitleLength => throw new ArgumentException(
                    $"Title cannot be longer than {MaxTitleLength} characters."),
                _ => title
            };
        }

        protected static string ValidateDescription(string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description);
            return description.Length switch
            {
                < 3 => throw new ArgumentException("Description must be at least 3 characters long."),
                > MaxContentLength => throw new ArgumentException(
                    $"Description cannot be longer than {MaxContentLength} characters."),
                _ => description
            };
        }
        // later I can put Domain in its own project and use internal for the Update method so that it can only be called from within the Domain project
        public virtual void Update(string? title, string? description, DateOnly? start, DateOnly? end)
        {
            if (title != null)
            {
                Title = ValidateTitle(title);
            }
            if (description != null)
            {
                Description = ValidateDescription(description);
            }
        }
    }
}
