namespace HealthJournal.Api.Domain.Exceptions;

public class EntryNotFoundException(string message) : DomainException(message)
{
    public override int StatusCode => 404;
}
