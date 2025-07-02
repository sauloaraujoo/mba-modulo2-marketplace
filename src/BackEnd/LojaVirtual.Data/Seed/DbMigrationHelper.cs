using LojaVirtual.Business.Entities;
using LojaVirtual.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LojaVirtual.Data.Seed
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
            var usuarioVendedor = new IdentityUser
            {
                Id = idUser.ToString(),
                Email = "vendedor@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "VENDEDOR@TESTE.COM",
                UserName = "vendedor@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==",
                NormalizedUserName = "VENDEDOR@TESTE.COM"
            };

            // Cria claim para usuário vendedor@teste.com, com as permisões de visualizar e mudar o Status dos Produtos
            var claimVendedorProdutos = new IdentityUserClaim<string>
            {
                UserId = idUser.ToString(),
                ClaimType = "Produtos",
                ClaimValue = "AD,VI,ED,EX,ATUALIZAR_STATUS"
            };
            await context.UserClaims.AddAsync(claimVendedorProdutos);

            var vendedor = new Vendedor(idUser, "Vendedor", usuarioVendedor.Email);
            var categoria = new Categoria("Informática", "Descrição da categoria Informática");
            var produto = new Produto("Mouse", "Descrição do produto Mouse", "mouse.jpg", 100, 20, true, categoria.Id);
            produto.VinculaVendedor(vendedor.Id);
            categoria.AddProduto(produto);

            produto = new Produto("Teclado", "Descrição do produto Teclado", "teclado.jpg", 150, 15, true, categoria.Id);
            produto.VinculaVendedor(vendedor.Id);
            categoria.AddProduto(produto);

            await context.Users.AddAsync(usuarioVendedor);
            await context.VendedorSet.AddAsync(vendedor);

            idUser = Guid.NewGuid();
            usuarioVendedor = new IdentityUser
            {
                Id = idUser.ToString(),
                Email = "vendedor2@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "VENDEDOR2@TESTE.COM",
                UserName = "vendedor2@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==",
                NormalizedUserName = "VENDEDOR2@TESTE.COM"
            };

             vendedor = new Vendedor(idUser, "Vendedor 2", usuarioVendedor.Email);

            produto = new Produto("Monitor", "Descrição do produto Monitor", "monitor.jpg", 899, 15, true, categoria.Id);
            produto.VinculaVendedor(vendedor.Id);
            categoria.AddProduto(produto);


            await context.CategoriaSet.AddAsync(categoria);

            // Cria claim para usuário vendedor@teste.com, com as permisões de visualizar e mudar o Status dos Produtos
            claimVendedorProdutos = new IdentityUserClaim<string>
            {
                UserId = idUser.ToString(),
                ClaimType = "Produtos",
                ClaimValue = "AD,VI,ED,EX,ATUALIZAR_STATUS"
            };
            await context.UserClaims.AddAsync(claimVendedorProdutos);


            await context.Users.AddAsync(usuarioVendedor);
            await context.VendedorSet.AddAsync(vendedor);

            var idClienteUser = Guid.NewGuid();
            var userCliente = new IdentityUser
            {
                Id = idClienteUser.ToString(),
                Email = "cliente@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "CLIENTE@TESTE.COM",
                UserName = "cliente@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==", //Teste@123
                NormalizedUserName = "CLIENTE@TESTE.COM"
            };

            var cliente = new Cliente(idClienteUser, "Cliente", "cliente@teste.com");
            cliente.AddFavorito(produto.Id);
            await context.Users.AddAsync(userCliente);
            await context.ClienteSet.AddAsync(cliente);
            
            // Cria user admin@teste.com
            var idAdminUser = Guid.NewGuid();
            var adminUser = new IdentityUser
            {
                Id = idAdminUser.ToString(),
                Email = "admin@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@TESTE.COM",
                UserName = "admin@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==", // Teste@123
                NormalizedUserName = "ADMIN@TESTE.COM"
            };
            await context.Users.AddAsync(adminUser);

            // Cria claim Clientes para usuário cliente@teste.com, com todas as permissões
            var claimCliente = new IdentityUserClaim<string>
            {
                UserId = idClienteUser.ToString(),
                ClaimType = "Clientes",
                ClaimValue = "VISUALIZAR_FAVORITOS,EDITAR_FAVORITOS"
            };
            await context.UserClaims.AddAsync(claimCliente);

            // Cria claim Categorias para usuário admin@teste.com, com todas as permissões
            var claimCategorias = new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Categorias",
                ClaimValue = "ADICIONAR,EDITAR,EXCLUIR"
            };
            await context.UserClaims.AddAsync(claimCategorias);

            // Cria claim para usuário admin@teste.com, com as permisões de visualizar e mudar o Status dos Vendedores
            var claimAdmVendedores = new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Vendedores",
                ClaimValue = "VI,ATUALIZAR_STATUS"
            };
            await context.UserClaims.AddAsync(claimAdmVendedores);


            // Cria claim para usuário admin@teste.com, com as permisões de visualizar e mudar o Status dos Produtos
            var claimAdmProdutos = new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Produtos",
                ClaimValue = "VI,TODOS_PRODUTOS,ATUALIZAR_STATUS"
            };
            await context.UserClaims.AddAsync(claimAdmProdutos);



            await context.SaveChangesAsync();
        }
    }
}
