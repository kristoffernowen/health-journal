// Global usings consolidated in GlobalUsings.cs
namespace HealthJournal.Api.Features.JournalWeeks;

public static class UpdateJournalWeekDescriptionEndpoint
{
    public static RouteGroupBuilder MapUpdateJournalWeekDescription(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id}", async (
                DataContext context,
                ILoggerFactory loggerFactory,
                IValidator<UpdateJournalWeekDto> validator,
                Guid id,
                UpdateJournalWeekDto input) =>
            {
                var logger = loggerFactory.CreateLogger("UpdateJournalWeekDescriptionEndpoint");

                var validationResult = await validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var fakeUser = FakeUserProvider.LoggedInDummy();
                var user = await context.JournalUsers
                    .Include(u => u.JournalWeeks)
                    .FirstAsync(u => u.ExtUserId == fakeUser.ExtUserId);

                user.UpdateWeekDescription(id, input.Description);

                context.JournalUsers.Update(user);
                await context.SaveChangesAsync();

                logger.LogInformation("Updated journal week description with ID {Id}", id);

                return Results.NoContent();
            })
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
