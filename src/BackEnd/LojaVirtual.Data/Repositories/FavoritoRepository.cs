using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repository
{
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly LojaVirtualContext _context;

        public FavoritoRepository(LojaVirtualContext context)
        {
            _context = context;
        }

        public async Task Insert(Favorito favorito, CancellationToken cancellationToken)
        {
            _context.FavoritoSet.Add(favorito);
            await Task.CompletedTask;
        }

        public async Task Remove(Favorito favorito, CancellationToken cancellationToken)
        {
            _context.FavoritoSet.Remove(favorito);
            await Task.CompletedTask;
        }

        public async Task<Favorito?> GetByClienteProduto(Guid clienteId, Guid produtoId, CancellationToken cancellationToken)
        {
            return await _context.FavoritoSet
                .FirstOrDefaultAsync(f => f.ClienteId == clienteId && f.ProdutoId == produtoId, cancellationToken);
        }

        public async Task<IEnumerable<Favorito>> ListByCliente(Guid clienteId, CancellationToken cancellationToken)
        {
            return await _context.FavoritoSet
                .Include(f => f.Produto)
                .Where(f => f.ClienteId == clienteId)
                .ToListAsync(cancellationToken);
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
