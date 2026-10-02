namespace HealthJournal.Api.Domain.Exceptions;

public class TitleTooShortException(string title) : DomainException($"Title {title} must be at least three characters long.")
{
    public override int StatusCode => 400;
}
