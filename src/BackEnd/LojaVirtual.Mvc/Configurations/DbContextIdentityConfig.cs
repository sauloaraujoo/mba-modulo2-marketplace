using LojaVirtual.Data.Context;
using LojaVirtual.Data.Helpers;
using LojaVirtual.Mvc.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Mvc.Configurations
{
    public static class DbContextIdentityConfig
    {
        public static IServiceCollection AddDbContextIdentityConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            var absolutePath = DbPathHelper.GetDatabasePath();

            services.AddDbContext<LojaVirtualContext>(options =>
            {
                options.UseSqlite($"Data Source={absolutePath}");
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()                
                .AddEntityFrameworkStores<LojaVirtualContext>()
                .AddErrorDescriber<IdentityPortuguesMsgError>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
