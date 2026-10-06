Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Bootstrapping application.");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddSerilog((lc) => lc
        .ReadFrom.Configuration(builder.Configuration)); // using ILoggerFactory in endpoints till handlers are in place

    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityEntryValidator>(ServiceLifetime.Transient);

    builder.Services.AddAuth0(builder.Configuration);

    builder.Services.AddOpenApi();
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseConfiguredSerilogRequestLogging();
    app.UseExceptionHandler();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapEndpoints();

    Log.Information("Application pipeline configured.");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application could not start");
}
finally
{
    Log.CloseAndFlush();
}
