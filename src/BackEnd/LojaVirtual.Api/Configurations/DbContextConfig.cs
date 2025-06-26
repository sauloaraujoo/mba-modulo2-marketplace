using LojaVirtual.Data.Context;
using LojaVirtual.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Api.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            var absolutePath = DbPathHelper.GetDatabasePath();

            services.AddDbContext<LojaVirtualContext>(options =>
            {
                options.UseSqlite($"Data Source={absolutePath}");
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
