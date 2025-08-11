using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly LojaVirtualContext _context;

        public ClienteRepository(LojaVirtualContext context)
        {
            _context = context;
        }
        public async Task<Cliente?> ObterClienteComFavoritos(Guid id, CancellationToken tokenDeCancelamento)
        {
            var cliente = await _context.ClienteSet
                .Include(c => c.Favoritos)
                    .ThenInclude(f => f.Produto)
                        .ThenInclude(p => p.Vendedor)
                .Include(c => c.Favoritos)
                    .ThenInclude(f => f.Produto)
                        .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(c => c.Id == id, tokenDeCancelamento);

            if (cliente != null)
            {
                var favoritosAtivos = cliente.Favoritos
                    .Where(f => f.Produto.Ativo && f.Produto.Vendedor != null && f.Produto.Vendedor.Ativo)
                    .ToList();

                cliente.SetFavoritos(favoritosAtivos);
            }

            return cliente;
        }
        public void Editar(Cliente cliente)
        {
            _context.ClienteSet.Update(cliente);
        }

        public async Task Inserir(Cliente cliente, CancellationToken tokenDeCancelamento)
        {
            var claimCliente = new IdentityUserClaim<string>
            {
                UserId = cliente.Id.ToString(),
                ClaimType = "Clientes",
                ClaimValue = "VISUALIZAR_FAVORITOS,EDITAR_FAVORITOS"
            };
            await _context.UserClaims.AddAsync(claimCliente);
            await _context.ClienteSet.AddAsync(cliente, tokenDeCancelamento);
        }

        public async Task<int> SalvarMudancas(CancellationToken tokenDeCancelamento)
        {
            return await _context.SaveChangesAsync(tokenDeCancelamento);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
