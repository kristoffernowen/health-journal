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
            services.AddScoped<IJournalEntryService, JournalEntryService>();
            return services;
        }

        public IServiceCollection AddAuth0(IConfiguration configuration)
        {
            services.AddAuth0ApiAuthentication(configuration.GetSection("Auth0"));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("JournalRead",
                    policy => policy.RequireClaim(
                        "permissions", "journal:read"));

                options.AddPolicy("JournalWrite",
                    policy => policy.RequireClaim(
                        "permissions", "journal:write"));

                
            });
            
            return services;
        }
    }
}
