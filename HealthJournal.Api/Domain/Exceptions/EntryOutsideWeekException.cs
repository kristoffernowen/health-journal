namespace HealthJournal.Api.Domain.Exceptions;

public class EntryOutsideWeekException(DateOnly start, DateOnly end, WeekOfYear week)
    : DomainException($"Entry from {start} to {end} is outside the week {week.Week} in year {week.Year}")
{
    public override int StatusCode => 400; // Bad Request
}
