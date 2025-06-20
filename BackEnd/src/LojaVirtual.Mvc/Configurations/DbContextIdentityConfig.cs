using LojaVirtual.Core.Infra.Context;
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
            services.AddDbContext<LojaVirtualContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
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
