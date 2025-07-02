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
        public async Task<Cliente?> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ClienteSet.Include(c => c.Favoritos)
                                                .ThenInclude(f => f.Produto)
                                                    .ThenInclude(p => p.Vendedor)
                                                .Include(c => c.Favoritos)
                                                    .ThenInclude(f => f.Produto)
                                                        .ThenInclude(p => p.Categoria)
                                                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
       }
        public void Update(Cliente cliente)
        {
            _context.ClienteSet.Update(cliente);
        }

        public async Task Insert(Cliente request, CancellationToken cancellationToken)
        {
            var claimCliente = new IdentityUserClaim<string>
            {
                UserId = request.Id.ToString(),
                ClaimType = "Clientes",
                ClaimValue = "VISUALIZAR_FAVORITOS,EDITAR_FAVORITOS"
            };
            await _context.UserClaims.AddAsync(claimCliente);
            await _context.ClienteSet.AddAsync(request, cancellationToken);
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
