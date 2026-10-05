namespace HealthJournal.Api.Features.JournalEntries.ActivityEntries;

public static class CreateActivityEntryEndpoint
{
    public static RouteGroupBuilder MapCreateActivityEntry(this RouteGroupBuilder group)
    {
        group.MapPost("/activity", async (
                IJournalEntryService journalEntryService,
                IValidator<InputCreateActivityEntryDto> validator,
                InputCreateActivityEntryDto input) =>
            {
                var validationResult = await validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var journalEntry = await journalEntryService.CreateJournalEntryAsync(input);

                return Results.Created($"/journal-entries/{journalEntry.Id}", journalEntry);
            })
            .WithName("CreateJournalEntry");
        return group;
    }
}

public record OutputCreateActivityEntryDto(Guid Id, string Title, string Description, DateTime CreatedAt, DateOnly PerformedAt);

public static class OutputCreateActivityEntryDtoExtensions
{
    public static OutputCreateActivityEntryDto ToOutputCreateActivityEntryDto(this ActivityEntry entry)
    {
        return new OutputCreateActivityEntryDto(entry.Id, entry.Title, entry.Description, entry.CreatedAt, entry.PerformedAt);
    }
}

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
