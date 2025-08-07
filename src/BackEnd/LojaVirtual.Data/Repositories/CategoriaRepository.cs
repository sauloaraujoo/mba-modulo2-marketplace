using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly LojaVirtualContext _context;

        public CategoriaRepository(LojaVirtualContext context)
        {
            _context = context;
        }

        public async Task Inserir(Categoria entity, CancellationToken tokenDeCancelamento)
        {
            await _context.CategoriaSet.AddAsync(entity, tokenDeCancelamento);
        }
        public Task Editar(Categoria entity, CancellationToken tokenDeCancelamento)
        {
            return Task.FromResult(_context.CategoriaSet.Update(entity));
        }

        public async Task<Categoria> ObterPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _context.CategoriaSet.FirstOrDefaultAsync(c => c.Id == id, tokenDeCancelamento);
        }

        public async Task<Categoria> ObterComProduto(Guid id, CancellationToken tokenDeCancelamento)
        {
            return await _context
                            .CategoriaSet
                            .Include(c => c.Produtos)
                            .FirstOrDefaultAsync(c => c.Id == id, tokenDeCancelamento);
        }
        public async Task<IList<Categoria>> ListarSemContexto(CancellationToken tokenDeCancelamento)
        {
            return await _context
                            .CategoriaSet
                            .AsNoTracking()
                            .ToListAsync(tokenDeCancelamento);
        }
        public async Task Remover(Categoria categoria, CancellationToken tokenDeCancelamento)
        {
            await Task.FromResult(_context.CategoriaSet.Remove(categoria)).ConfigureAwait(false);
        }
        public async Task<bool> Existe(string nome, CancellationToken tokenDeCancelamento)
        {
            return await _context.CategoriaSet.AnyAsync(c => c.Nome == nome, tokenDeCancelamento);
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
