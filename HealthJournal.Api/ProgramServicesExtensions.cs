namespace HealthJournal.Api;

public static class ProgramServicesExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            services.AddDbContext<DataContext>(opt =>
                opt.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 1,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null)));

            return services;
        }

        public IServiceCollection AddApplicationServices()
        {
            services.AddScoped<IJournalWeekService, JournalWeekService>();
            return services;
        }
    }
}
