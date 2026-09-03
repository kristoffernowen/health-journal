namespace HealthJournal.Api.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ExtUserId { get; set; } = string.Empty;
    }
}
