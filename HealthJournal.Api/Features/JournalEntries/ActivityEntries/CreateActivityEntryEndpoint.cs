using FluentValidation;
using HealthJournal.Api.Domain.Journal;
using HealthJournal.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthJournal.Api.Features.JournalEntries.ActivityEntries
{
    public static class CreateActivityEntryEndpoint
    {
        public static RouteGroupBuilder MapCreateActivityEntry(this RouteGroupBuilder group)
        {
            group.MapPost("/activity", async (DataContext context, IValidator<InputCreateActivityEntryDto> validator, InputCreateActivityEntryDto input) =>
                {
                    var validationResult = await validator.ValidateAsync(input);
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }

                    var fakeUser = FakeUserProvider.LoggedInDummy();
                    var user = await context.JournalUsers
                        .Include(u => u.JournalWeeks)
                        .ThenInclude(jw => jw.Entries)
                        .FirstOrDefaultAsync(u => u.Id == fakeUser.Id);

                    if (user == null)
                    {
                        return Results.NotFound();
                    }

                    var journalEntry = ActivityEntry.Create(input.Title, input.Description, input.PerformedAt);
                    user.AddEntry(journalEntry);

                    context.ActivityEntries.Add(journalEntry);
                    await context.SaveChangesAsync();

                    return Results.Created($"/journal-entries/{journalEntry.Id}",
                        new OutputCreateActivityEntryDto(journalEntry.Id, journalEntry.Title, journalEntry.Description,
                            journalEntry.CreatedAt, journalEntry.PerformedAt));
                })
                .WithName("CreateJournalEntry");
            return group;
        }
    }

    public record OutputCreateActivityEntryDto(Guid Id, string Title, string Description, DateTime CreatedAt, DateOnly PerformedAt);

    public record InputCreateActivityEntryDto(string Title, string Description, DateOnly PerformedAt);

    public class CreateActivityEntryValidator : AbstractValidator<InputCreateActivityEntryDto>
    {
        public CreateActivityEntryValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
            RuleFor(x => x.PerformedAt)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Performed date cannot be in the future.");
        }
    }
}
