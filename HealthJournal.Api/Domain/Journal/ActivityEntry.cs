namespace HealthJournal.Api.Domain.Journal
{
    public class ActivityEntry : JournalEntryBase
    {
        public required DateOnly PerformedAt { get; set; }
        public override DateOnly Start => PerformedAt;
        public override DateOnly End => PerformedAt;

        public static ActivityEntry Create(string title, string description, DateOnly performedAt)
        {
            return new ActivityEntry
            {
                Title = ValidateTitle(title),
                Description = ValidateDescription(description),
                PerformedAt = performedAt
            };
        }

        public override void Update(string? title, string? description, DateOnly? start, DateOnly? end)
        {
            if (start != end)
            {
                throw new ArgumentException("For ActivityEntry, start and end dates must be the same.");
            }
            base.Update(title, description, null, null);
            PerformedAt = start ?? PerformedAt;
        }
    }
}
