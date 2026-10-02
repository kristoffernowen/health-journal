namespace HealthJournal.Api.Domain.Exceptions;

public class DescriptionTooLongException(string message) : DomainException(message)
{
    public override int StatusCode => 400; // Bad Request
}
