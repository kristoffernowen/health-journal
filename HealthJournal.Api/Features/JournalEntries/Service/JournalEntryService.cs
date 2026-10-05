namespace HealthJournal.Api.Features.JournalEntries.Service;

public class JournalEntryService(DataContext context, ILogger<JournalEntryService> logger) : IJournalEntryService
{
    public async Task<List<OutputGetJournalEntriesDto>> GetJournalEntriesAsync()
    {
        var user = FakeUserProvider.LoggedInDummy();
        var journalEntries = await context.JournalUsers.Include(u => u.JournalWeeks)
            .ThenInclude(jw => jw.Entries)
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.JournalWeeks)
            .SelectMany(jw => jw.Entries)
            .ToListAsync();

        return journalEntries.Select(
            j => new OutputGetJournalEntriesDto(j.Id, j.Title, j.Description, j.GetType().Name, j.CreatedAt)).ToList();
    }

    public async Task<OutputGetJournalEntryDto?> GetJournalEntryAsync(Guid id)
    {
        //needs user checking, but for now just return the entry if it exists
        var journalEntry = await context.JournalEntries.FindAsync(id);
        return journalEntry?.ToOutputGetJournalEntryDto();
    }

    public async Task DeleteJournalEntryAsync(Guid id)
    {
        var fakeUser = FakeUserProvider.LoggedInDummy();
        var user = await context.JournalUsers
            .Include(u => u.JournalWeeks)
            .ThenInclude(jw => jw.Entries)
            .FirstOrDefaultAsync(u => u.ExtUserId == fakeUser.ExtUserId);

        if (user == null)
        {
            throw new Exception($"User with ExtUserId {fakeUser.ExtUserId} not found.");
        }

        user.RemoveEntry(id);

        await context.SaveChangesAsync();

        logger.LogInformation("Deleted journal entry with ID {JournalEntryId} for user {UserId}", id, user.Id);
    }

    public async Task<OutputCreateActivityEntryDto> CreateJournalEntryAsync(InputCreateActivityEntryDto entry)
    {
        var fakeUser = FakeUserProvider.LoggedInDummy();
        var user = await context.JournalUsers
            .Include(u => u.JournalWeeks)
            .ThenInclude(jw => jw.Entries)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == fakeUser.Id);

        if (user == null)
        {
            throw new Exception($"User with ID {fakeUser.Id} not found.");
        }

        var journalEntry = ActivityEntry.Create(entry.Title, entry.Description, entry.PerformedAt);
        user.AddEntry(journalEntry);

        context.ActivityEntries.Add(journalEntry);
        await context.SaveChangesAsync();

        logger.LogInformation("Created journal entry with ID {JournalEntryId} for user {UserId}", journalEntry.Id, user.Id);

        return journalEntry.ToOutputCreateActivityEntryDto();
    }

    public async Task UpdateJournalEntryAsync(Guid id, InputUpdateActivityEntryDto entry)
    {
        var fakeUser = FakeUserProvider.LoggedInDummy();
        var user = await context.JournalUsers
            .Include(u => u.JournalWeeks)
            .ThenInclude(jw => jw.Entries)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.ExtUserId == fakeUser.ExtUserId);

        if (user == null)
        {
            throw new Exception($"User with ExtUserId {fakeUser.ExtUserId} not found.");
        }

        user.UpdateEntry(id, entry.Title, entry.Description, entry.PerformedAt, entry.PerformedAt);

        context.JournalUsers.Update(user);
        await context.SaveChangesAsync();

        logger.LogInformation("Updated activity entry with ID {Id}", id);
    }
}
