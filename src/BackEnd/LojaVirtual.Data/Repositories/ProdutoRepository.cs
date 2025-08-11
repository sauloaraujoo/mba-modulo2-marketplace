using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly LojaVirtualContext _context;

        public ProdutoRepository(LojaVirtualContext context)
        {
            _context = context;
        }
        public async Task Inserir(Produto entity, CancellationToken cancellationToken)
        {
            await _context.ProdutoSet.AddAsync(entity, cancellationToken);
        }
        public Task Editar(Produto entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.ProdutoSet.Update(entity));
        }
        public async Task<IEnumerable<Produto>> ObterTodosProdutosPropriosComCategoria(Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .Where(p => p.VendedorId == vendedorId)
                .OrderBy(p => p.Nome)
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Produto>> ObterTodosProdutosComCategoria( CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .OrderBy(p => p.Categoria.Nome)
                .ThenBy(p => p.Nome)
                .ToListAsync(cancellationToken);
        }
        public async Task<Produto> ObterProprioComCategoriaPorId(Guid id, Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id && p.VendedorId == vendedorId, cancellationToken);
        }

        public async Task<Produto> ObterComCategoriaPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id , cancellationToken);
        }
        public async Task<Produto> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<Produto> ObterProdutoProprioPorId(Guid id, Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .FirstOrDefaultAsync(c => c.Id == id && c.VendedorId == vendedorId, cancellationToken);
        }

        public async Task<List<Produto>> ListarComCategoriaVendedorSemContexto(CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.Ativo == true && p.Vendedor.Ativo == true)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Produto>> ListarComCategoriaVendedorPorCategoriaSemContexto(Guid categoriaId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.CategoriaId == categoriaId && p.Ativo == true && p.Vendedor.Ativo == true)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Produto>> ListarComCategoriaVendedorPorVendedorSemContexto(Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.VendedorId == vendedorId && p.Ativo && p.Vendedor.Ativo)
                .ToListAsync(cancellationToken);
        }

        public async Task<Produto> ObterProdutoComCategoriaVendedorPorId(Guid produtoId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == produtoId && p.Ativo == true && p.Vendedor.Ativo == true, cancellationToken);
        }

        public async Task Remover(Produto produto, CancellationToken cancellationToken)
        {
            Task.FromResult(_context.ProdutoSet.Remove(produto));
        }
        public async Task<bool> Existe(string nome, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet.AnyAsync(c => c.Nome == nome, cancellationToken);
        }

        public async Task<List<Produto>> Listar(Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Where(p => p.VendedorId == vendedorId)
                .ToListAsync(cancellationToken);            
        }
        public async Task<int> SalvarMudancas(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<Produto>> ListarComCategoriaVendedorPaginadoSemContexto(int pagina, int tamanho, CancellationToken cancellationToken)
        {
            var query = _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.Ativo == true && p.Vendedor.Ativo == true);

            var totalItens = await query.CountAsync();

            var itens = await query
            .OrderBy(p => p.Nome)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync(cancellationToken);

            return new PagedResult<Produto>
            {
                TotalItens = totalItens,
                PaginaAtual = pagina,
                TamanhoPagina = tamanho,
                Itens = itens
            };
        }

        public async Task<PagedResult<Produto>> ListarComCategoriaVendedorPorCategoriaPaginadoSemContexto(Guid categoriaId, int pagina, int tamanho, CancellationToken cancellationToken)
        {
            var query = _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.CategoriaId == categoriaId && p.Ativo == true && p.Vendedor.Ativo == true);

            var totalItens = await query.CountAsync();

            var itens = await query
            .OrderBy(p => p.Nome)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync(cancellationToken);

            return new PagedResult<Produto>
            {
                TotalItens = totalItens,
                PaginaAtual = pagina,
                TamanhoPagina = tamanho,
                Itens = itens
            };
        }

        public async Task<PagedResult<Produto>> ListarComCategoriaVendedorPorVendedorPaginadoSemContexto(Guid vendedorId, int pagina, int tamanho, CancellationToken cancellationToken)
        {
            var query = _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.VendedorId == vendedorId && p.Ativo && p.Vendedor.Ativo);

            var totalItens = await query.CountAsync();

            var itens = await query
            .OrderBy(p => p.Nome)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync(cancellationToken);

            return new PagedResult<Produto>
            {
                TotalItens = totalItens,
                PaginaAtual = pagina,
                TamanhoPagina = tamanho,
                Itens = itens
            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
