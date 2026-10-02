namespace HealthJournal.Api.Domain.Exceptions;

public class ActivityStartEndMismatchException(string message) : DomainException(message)
{
    public override int StatusCode => 400;
}
