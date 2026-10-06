namespace HealthJournal.Api.Features.JournalEntries.ActivityEntries;

public static class UpdateActivityEntryEndpoint
{
    public static RouteGroupBuilder MapUpdateActivityEntry(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}", async (
                IJournalEntryService journalEntryService,
                IValidator<InputUpdateActivityEntryDto> validator,
                Guid id,
                InputUpdateActivityEntryDto input) =>
            {
                var validationResult = await validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                await journalEntryService.UpdateJournalEntryAsync(id, input);

                return Results.NoContent();
            })
            .RequireAuthorization("JournalWrite")
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
