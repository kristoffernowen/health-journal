using System.Diagnostics.CodeAnalysis;

namespace HealthJournal.Api.Models
{
    public class JournalEntry(string title, string content, Guid userId)
    {
        private const int MaxTitleLength = 100;
        private const int MaxContentLength = 1000;

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Title { get; set; } = ValidateTitle(title);
        public string Content { get; set; } = ValidateContent(content);
        public Guid UserId { get; set; } = userId;

        private static string ValidateTitle(string title)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            if(title.Length > MaxTitleLength)
            {
                throw new ArgumentException($"Title cannot be longer than {MaxTitleLength} characters.");
            }
            return title;
        }

        private static string ValidateContent(string content)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content);
            if(content.Length > MaxContentLength)
            {
                throw new ArgumentException($"Content cannot be longer than {MaxContentLength} characters.");
            }
            return content;
        }
    }
}
