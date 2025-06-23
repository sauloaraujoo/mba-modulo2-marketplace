namespace LojaVirtual.Api.Configurations
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(cors =>
            {
                var origins = configuration.GetSection("CorsOrigins")
                    .GetChildren()
                    .ToArray()
                    .Select(c => c.Value)
                    .ToArray();
                Console.WriteLine("Iniciando cors {0}", origins?.Aggregate((a, p) => $"{a}, {p}"));
                cors.AddPolicy("CorsPolicy", pol =>
                {
                    pol
                     .WithHeaders("Origin", "X-Requested-With", "x-xsrf-token", "Content-Type", "Accept", "Authorization")
                     .WithOrigins(origins)
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials()
                     .SetIsOriginAllowedToAllowWildcardSubdomains()
                     .SetPreflightMaxAge(TimeSpan.FromDays(10))
                     .Build();
                });
            });

            return services;
        }
    }
}
