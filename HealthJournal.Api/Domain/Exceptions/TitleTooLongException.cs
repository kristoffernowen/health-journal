namespace HealthJournal.Api.Domain.Exceptions;

public class TitleTooLongException(string title, int maxTitleLength) : DomainException($"Title {title} cannot be longer than {maxTitleLength} characters.")
{
    public override int StatusCode => 400;
}
