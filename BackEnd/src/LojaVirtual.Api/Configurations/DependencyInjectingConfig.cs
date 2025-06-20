using LojaVirtual.Core.Business.Extensions.IdentityUser;
using LojaVirtual.Core.Business.Interfaces;
using LojaVirtual.Core.Business.Notifications;
using LojaVirtual.Core.Business.Services;
using LojaVirtual.Core.Infra.Repositories;

namespace LojaVirtual.Api.Configurations
{
    public static class DependencyInjectingConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IVendedorRepository, VendedorRepository>();

            //Notification
            services.AddScoped<INotifiable, Notifiable>();

            //Services
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProdutoService, ProdutoService>();

            services.AddScoped<IAppIdentifyUser, AppIdentityUser>();

            return services;
        }
    }
}
