using LojaVirtual.Core.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Api.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<LojaVirtualContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
