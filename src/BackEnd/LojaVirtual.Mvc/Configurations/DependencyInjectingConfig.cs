using LojaVirtual.Business.Extensions.IdentityUser;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notifications;
using LojaVirtual.Business.Services;
using LojaVirtual.Data.Repositories;
using LojaVirtual.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace LojaVirtual.Mvc.Configurations
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
            services.AddScoped<IVendedorService, VendedorService>();
            services.AddScoped<IAppIdentifyUser, AppIdentityUser>();

            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();
            return services;
        }
    }
}
