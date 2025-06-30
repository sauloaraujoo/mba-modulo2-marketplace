using LojaVirtual.Business.Extensions.IdentityUser;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notifications;
using LojaVirtual.Business.Services;
using LojaVirtual.Data.Repositories;
using LojaVirtual.Data.Repository;

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
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFavoritoRepository, FavoritoRepository>();

            //Notification
            services.AddScoped<INotifiable, Notifiable>();

            //Services
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IFavoritoService, FavoritoService>();
            services.AddScoped<IAppIdentifyUser, AppIdentityUser>();

            return services;
        }
    }
}
