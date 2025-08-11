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
            new Categoria("Celulares", "Descrição da categoria Celulares"),
            new Categoria("Tablets", "Descrição da categoria Tablets"),
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
                    ClaimValue = "VISUALIZAR,ADICIONAR,EDITAR,EXCLUIR,ATUALIZAR_STATUS"
                };
                await context.UserClaims.AddAsync(claimVendedorProdutos);

                int categoriaIndex = 0;


                var produtosLoja = new List<(string Nome, string Descricao, string Imagem, decimal Preco, int Quantidade, Guid CategoriaId)>();

                switch (v)
                {
                    case 1:
                        // Adiciona produtos da categoria Celulares para o Vendedor 1
                        produtosLoja.AddRange(new List<(string Nome, string Descricao, string Imagem, decimal Preco, int Quantidade, Guid CategoriaId)>
                        {
                            ("iPhone 16 256GB", "Mais detalhes do produto iPhone 16 256GB", "iphone16.jpg", 7799.00m, 20, categorias[0].Id),
                            ("iPhone 16 PRO 256GB", "Mais detalhes do produto iPhone 16 PRO 256GB", "iphone16pro.jpg", 10499.00m, 50, categorias[0].Id)
                        });
                        break;
                    case 2:
                        // Adiciona produtos da categoria Celulares para o Vendedor 2
                        produtosLoja.AddRange(new List<(string Nome, string Descricao, string Imagem, decimal Preco, int Quantidade, Guid CategoriaId)>
                        {
                            ("Samsung S24 Ultra 256GB", "Mais detalhes do produto Samsung S24 Ultra 256GB", "samsungs24.jpg", 5219.13m, 20, categorias[0].Id),
                            ("Samsung S25 Ultra 256GB", "Mais detalhes do produto Samsung S25 Ultra 256GB", "samsungs25.jpg", 8299.00m, 50, categorias[0].Id)
                        });
                        break;
                    case 3:
                        // Adiciona produtos da categoria Celulares para o Vendedor 3
                        produtosLoja.AddRange(new List<(string Nome, string Descricao, string Imagem, decimal Preco, int Quantidade, Guid CategoriaId)>
                        {
                            ("Motorola Edge 60 Fusion 256GB", "Mais detalhes do produto Motorola Edge 60 Fusion 256GB", "motorolaedge60.jpg", 2099.90m, 20, categorias[0].Id),
                            ("Motorola Moto G75 5g 256gb", "Mais detalhes do produto Motorola Moto G75 5g 256gb", "motog75.jpg", 1656.95m, 50, categorias[0].Id)
                        });
                        break;
                    case 4:
                        // Adiciona produtos da categoria Tablet para o Vendedor 4
                        produtosLoja.AddRange(new List<(string Nome, string Descricao, string Imagem, decimal Preco, int Quantidade, Guid CategoriaId)>
                        {
                            ("iPad Apple 10th 64GB", "Mais detalhes do produto iPad Apple 10th 64GB", "ipad10.jpg", 6899.50m, 20, categorias[1].Id),
                            ("iPad 11th 128GB", "Mais detalhes do produto iPad 11th 128GB", "ipad11.jpg", 8599.79m, 50, categorias[1].Id)
                        });
                        break;
                    case 5:
                        // Adiciona produtos da categoria Tablet para o Vendedor 5
                        produtosLoja.AddRange(new List<(string Nome, string Descricao, string Imagem, decimal Preco, int Quantidade, Guid CategoriaId)>
                        {
                            ("Samsung Galaxy Tab A9+ 128GB", "Mais detalhes do produto Samsung Galaxy Tab A9+ 128GB", "galaxytabA9.jpg", 1831.20m, 20, categorias[1].Id),
                            ("Galaxy Tab S10 FE 128GB", "Mais detalhes do produto Galaxy Tab S10 FE 128GB", "galaxytabS10.jpg", 2799.01m, 50, categorias[1].Id)
                        });
                        break;
                    default:
                        break;
                }

                for (int p = 0; p < produtosLoja.Count; p++)
                {
                    var categoria = categorias[categoriaIndex];

                    var produtoReal = produtosLoja[p];

                    var produto = new Produto(
                        produtoReal.Nome,
                        produtoReal.Descricao,
                        produtoReal.Imagem,
                        produtoReal.Preco,
                        produtoReal.Quantidade,
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
                ClaimValue = "VISUALIZAR,ADICIONAR,EDITAR,EXCLUIR"
            },
            new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Vendedores",
                ClaimValue = "VISUALIZAR,ATUALIZAR_STATUS"
            },
            new IdentityUserClaim<string>
            {
                UserId = idAdminUser.ToString(),
                ClaimType = "Produtos",
                ClaimValue = "VISUALIZAR,TODOS_PRODUTOS,ATUALIZAR_STATUS"
            }
          };
            await context.UserClaims.AddRangeAsync(claimsAdmin);
            await context.SaveChangesAsync();
        }

    }
}
