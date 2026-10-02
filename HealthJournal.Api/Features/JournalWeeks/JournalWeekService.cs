namespace HealthJournal.Api.Features.JournalWeeks;

public class JournalWeekService(DataContext context, ILogger<JournalWeekService> logger) : IJournalWeekService
{
    private readonly JournalUser _user = FakeUserProvider.LoggedInDummy();
    public async Task<JournalWeek?> GetJournalWeekAsync(Guid id)
    {
        var journalWeek = await context.JournalWeeks
            .Include(jw => jw.Entries)
            .FirstOrDefaultAsync(jw => jw.Id == id && jw.JournalUserId == _user.Id);

        return journalWeek;
    }

    public async Task<List<JournalWeek>> GetAllJournalWeeksAsync()
    {
        var journalWeeks = await context.JournalWeeks
            .Include(jw => jw.Entries)
            .Where(jw => jw.JournalUserId == _user.Id)
            .ToListAsync();
        return journalWeeks;
    }

    public async Task<bool> UpdateJournalWeekDescriptionAsync(Guid id, string? description)
    {
        var user = await context.JournalUsers
            .Include(u => u.JournalWeeks)
            .FirstAsync(u => u.Id == _user.Id);
        user.UpdateWeekDescription(id, description);
        await context.SaveChangesAsync();
        // var journalWeek = await context.JournalWeeks
        //     .FirstOrDefaultAsync(jw => jw.Id == id && jw.JournalUserId == _user.Id);
        // if (journalWeek == null)
        // {
        //     return false;
        // }
        // journalWeek.UpdateDescription(description);
        // context.JournalWeeks.Update(journalWeek);
        // await context.SaveChangesAsync();

        logger.LogInformation("Updated journal week description with ID {Id}", id);
        
        return true;
    }
}
