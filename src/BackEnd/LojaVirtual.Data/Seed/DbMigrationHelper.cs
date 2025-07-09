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
            if (await context.CategoriaSet.AnyAsync()) return;

            var categorias = new List<Categoria>
            {
            new Categoria("Informática", "Descrição da categoria Informática"),
            new Categoria("Eletrodomésticos", "Descrição da categoria Eletrodomésticos"),
            new Categoria("Celulares", "Descrição da categoria Celulares"),
            new Categoria("Áudio e Vídeo", "Descrição da categoria Áudio e Vídeo"),
            new Categoria("Eletroportáteis", "Descrição da categoria Eletroportáteis")
            };

            await context.CategoriaSet.AddRangeAsync(categorias);

            for (int v = 1; v <= 5; v++)
            {
                var idUser = Guid.NewGuid();
                var emailVendedor = $"vendedor{v}@teste.com";

                var usuarioVendedor = new IdentityUser
                {
                    Id = idUser.ToString(),
                    Email = emailVendedor,
                    EmailConfirmed = true,
                    NormalizedEmail = emailVendedor.ToUpper(),
                    UserName = emailVendedor,
                    AccessFailedCount = 0,
                    PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==", // Teste@123
                    NormalizedUserName = emailVendedor.ToUpper()
                };
                await context.Users.AddAsync(usuarioVendedor);

                var vendedor = new Vendedor(idUser, $"Vendedor {v}", emailVendedor);
                await context.VendedorSet.AddAsync(vendedor);

                // Claim do vendedor
                var claimVendedorProdutos = new IdentityUserClaim<string>
                {
                    UserId = idUser.ToString(),
                    ClaimType = "Produtos",
                    ClaimValue = "AD,VI,ED,EX,ATUALIZAR_STATUS"
                };
                await context.UserClaims.AddAsync(claimVendedorProdutos);

                // Criação de 5 produtos, distribuindo entre categorias
                int categoriaIndex = 0;
                for (int p = 1; p <= 5; p++)
                {
                    var categoria = categorias[categoriaIndex];
                    var produto = new Produto(
                            $"Produto {v}-{p}",
                            $"Descrição do Produto {v}-{p}",
                            $"imagem{p}.jpg",
                            100 + p,
                            10 + p,
                            true,
                            categoria.Id
                        );
                    produto.VinculaVendedor(vendedor.Id);
                    categoria.AddProduto(produto);

                    categoriaIndex++;

                    if (categoriaIndex >= categorias.Count)
                        categoriaIndex = 0;
                }
            }

            var idClienteUser = Guid.NewGuid();
            var userCliente = new IdentityUser
            {
                Id = idClienteUser.ToString(),
                Email = "cliente@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "CLIENTE@TESTE.COM",
                UserName = "cliente@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEF/nmfwFGPa8pnY9AvZL8HKI7r7l+aM4nryRB+Y3Ktgo6d5/0d25U2mhixnO4h/K5w==", // Teste@123
                NormalizedUserName = "CLIENTE@TESTE.COM"
            };
            await context.Users.AddAsync(userCliente);

            var cliente = new Cliente(idClienteUser, "Cliente", "cliente@teste.com");
            
            Produto? primeiroProduto = null;

            foreach (var categoria in categorias)
            {
                if (categoria.Produtos.Any())
                {
                    primeiroProduto = categoria.Produtos.First();
                    break;
                }
            }

            if (primeiroProduto != null)
            {
                cliente.AddFavorito(primeiroProduto.Id);
            }

            await context.ClienteSet.AddAsync(cliente);

            var claimCliente = new IdentityUserClaim<string>
            {
                UserId = idClienteUser.ToString(),
                ClaimType = "Clientes",
                ClaimValue = "VISUALIZAR_FAVORITOS,EDITAR_FAVORITOS"
            };
            await context.UserClaims.AddAsync(claimCliente);

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

            var claimsAdmin = new List<IdentityUserClaim<string>>
          {
            new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Categorias",
                ClaimValue = "VI,AD,ED,EX"
            },
            new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Vendedores",
                ClaimValue = "VI,ATUALIZAR_STATUS"
            },
            new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Produtos",
                ClaimValue = "VI,TODOS_PRODUTOS,ATUALIZAR_STATUS"
            }
          };
            await context.UserClaims.AddRangeAsync(claimsAdmin);
            await context.SaveChangesAsync();
        }

    }
}
