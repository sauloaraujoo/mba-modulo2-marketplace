using LojaVirtual.Api.Extensions;
using LojaVirtual.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace LojaVirtual.Api.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityPortuguesMsgError>()
                .AddEntityFrameworkStores<LojaVirtualContext>();
            return services;
        }
    }
}
