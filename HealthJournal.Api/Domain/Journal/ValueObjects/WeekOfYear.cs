namespace HealthJournal.Api.Domain.Journal.ValueObjects
{
    public readonly record struct WeekOfYear(int Year, int Week)
    {
        public static WeekOfYear FromDate(DateOnly date)
        {
            var dateTime = date.ToDateTime(TimeOnly.MinValue);
            return new WeekOfYear(System.Globalization.ISOWeek.GetYear(dateTime), System.Globalization.ISOWeek.GetWeekOfYear(dateTime));
        }

        public (DateOnly Start, DateOnly End) GetSpan()
        {
            var start = System.Globalization.ISOWeek.ToDateTime(Year, Week, DayOfWeek.Monday);
            var end = start.AddDays(6);
            return (DateOnly.FromDateTime(start), DateOnly.FromDateTime(end));
        }

        public bool Contains(DateOnly date)
        {
            var (start, end) = GetSpan();
            return date >= start && date <= end;
        }
    }
}
