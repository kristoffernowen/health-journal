// Global usings consolidated in GlobalUsings.cs
using System.Diagnostics;

namespace HealthJournal.Api.Features.JournalEntries.ActivityEntries;

public static class UpdateActivityEntryEndpoint
{
    public static RouteGroupBuilder MapUpdateActivityEntry(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}", async (
                DataContext context,
                ILoggerFactory loggerFactory,
                IValidator<InputUpdateActivityEntryDto> validator, 
                Guid id, 
                InputUpdateActivityEntryDto input) =>
            {
                var logger = loggerFactory.CreateLogger("UpdateActivityEntryEndpoint");

                var validationResult = await validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var fakeUser = FakeUserProvider.LoggedInDummy();
                var user = await context.JournalUsers
                    .Include(u => u.JournalWeeks)
                    .ThenInclude(jw => jw.Entries)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(u => u.ExtUserId == fakeUser.ExtUserId);

                Debug.Assert(user != null, nameof(user) + " != null");
                user.UpdateEntry(id, input.Title, input.Description, input.PerformedAt, input.PerformedAt);

                context.JournalUsers.Update(user);
                await context.SaveChangesAsync();

                logger.LogInformation("Updated activity entry with ID {Id}", id);
                
                return Results.NoContent();
            })
            .WithName("UpdateJournalEntry");
        return group;
    }
}

public record InputUpdateActivityEntryDto(string? Title, string? Description, DateOnly? PerformedAt);

public class UpdateActivityEntryValidator : AbstractValidator<InputUpdateActivityEntryDto>
{
    public UpdateActivityEntryValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
        RuleFor(x => x.PerformedAt)
            .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Now)).WithMessage("Performed date cannot be in the future.");
    }
}