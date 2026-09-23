// Global usings consolidated in GlobalUsings.cs

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting.");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((lc) =>lc
        .ReadFrom.Configuration(builder.Configuration)
        ); // using ILoggerFactory in endpoints till handlers are in place


    var connectionString = builder.Configuration.GetConnectionString("Postgres");
    builder.Services.AddDbContext<DataContext>(opt =>
        opt.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 1,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null)));

    builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityEntryValidator>(ServiceLifetime.Transient);

    builder.Services.AddOpenApi();
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    var journalWeek = app.MapGroup("/journal-weeks")
        .WithTags("Journal Weeks")
        .WithGroupName("Journal Weeks")
        .WithDescription("Endpoints for managing journal weeks and journal entries in week views");

    journalWeek.MapGetJournalWeeks();
    journalWeek.MapGetJournalWeekById();
    journalWeek.MapUpdateJournalWeekDescription();

    var journalEntry = app.MapGroup("/journal-entries")
        .WithTags("Journal Entries")
        .WithGroupName("Journal Entries")
        .WithDescription("Endpoints for managing journal entries regardless of weeks");
    // per type of entry, we can have different endpoints for creating and updating them, but the retrieval and deletion can be generic
    journalEntry.MapCreateActivityEntry();
    journalEntry.MapUpdateActivityEntry();

    journalEntry.MapGetJournalEntry();
    journalEntry.MapGetJournalEntries();
    journalEntry.MapDeleteJournalEntry();

    app.UseSerilogRequestLogging();

    Log.Information("Application started successfully");

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