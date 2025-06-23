using LojaVirtual.Core.Business.Entities;
using LojaVirtual.Core.Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LojaVirtual.Core.Infra.Seed
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelper.EnsureSeedData(app).Wait();
        }
    }
    public static class DbMigrationHelper
    {
        public static async Task EnsureSeedData(WebApplication application)
        {
            var services = application.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<LojaVirtualContext>();

            if (env.EnvironmentName == "Development")

            {
                await context.Database.MigrateAsync();

                await EnsureSeedTables(context);
            }
        }
        private static async Task EnsureSeedTables(LojaVirtualContext context)
        {
            if (context.CategoriaSet.Any()) return;

            var idUser = Guid.NewGuid();

            var email = "admin@marketplace.com";
            var senha = "Abcd1234!";

            var usuario = new IdentityUser
            {
                Id = idUser.ToString(),
                Email = email,
                EmailConfirmed = true,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                AccessFailedCount = 0,
                NormalizedUserName = email.ToUpper()
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();
            usuario.PasswordHash = passwordHasher.HashPassword(usuario, senha);

            var vendedor = new Vendedor(idUser, "Vendedor Teste", usuario.Email);
            var categoria = new Categoria("Informática", "Descrição da categoria Informática");
            var produto = new Produto("Mouse", "Descrição do produto Mouse", "mouse.jpg", 100, 20, categoria.Id);
            produto.VinculaVendedor(vendedor.Id);
            categoria.AddProduto(produto);

            await context.Users.AddAsync(usuario);
            await context.VendedorSet.AddAsync(vendedor);
            await context.CategoriaSet.AddAsync(categoria);

            await context.SaveChangesAsync();
        }
    }
}
