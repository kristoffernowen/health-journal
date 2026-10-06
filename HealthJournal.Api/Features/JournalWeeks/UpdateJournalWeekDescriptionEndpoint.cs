namespace HealthJournal.Api.Features.JournalWeeks;

public static class UpdateJournalWeekDescriptionEndpoint
{
    public static RouteGroupBuilder MapUpdateJournalWeekDescription(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id}", async (
                IJournalWeekService journalWeekService,
                IValidator<UpdateJournalWeekDto> validator,
                Guid id,
                UpdateJournalWeekDto input) =>
            {
                var validationResult = await validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var updated = await journalWeekService.UpdateJournalWeekDescriptionAsync(id, input.Description);
                return updated ? Results.NoContent() : Results.NotFound();
            })
            .RequireAuthorization("JournalWrite")
            .WithName("UpdateJournalWeek")
            .Produces(StatusCodes.Status204NoContent);
        return group;
    }
}

public record UpdateJournalWeekDto(string Description);

public class UpdateJournalWeekValidator : AbstractValidator<UpdateJournalWeekDto>
{
    public UpdateJournalWeekValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}
