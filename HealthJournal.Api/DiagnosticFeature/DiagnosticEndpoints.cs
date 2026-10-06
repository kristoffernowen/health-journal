namespace HealthJournal.Api.DiagnosticFeature;

public static class DiagnosticEndpoints
{
    public static RouteGroupBuilder MapDiagnosticEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/claims", (HttpContext httpContext) =>
        {
            var claims = httpContext.User.Claims.Select(c => new
            {
                c.Type,
                c.Value
            });

            return Results.Ok(claims);
        }).RequireAuthorization("Diagnose");

        group.MapGet("/whoami", (HttpContext context) => Results.Ok(new
            {
                IsAuthenticated = context.User.Identity?.IsAuthenticated,
                Name = context.User.Identity?.Name,
                Claims = context.User.Claims.Count()
            }))
            .RequireAuthorization("Diagnose");

        return group;
    }
}


