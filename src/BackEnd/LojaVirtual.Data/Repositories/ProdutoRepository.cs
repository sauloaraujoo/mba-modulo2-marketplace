﻿using LojaVirtual.Business.Common;
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
        public async Task Insert(Produto entity, CancellationToken cancellationToken)
        {
            await _context.ProdutoSet.AddAsync(entity, cancellationToken);
        }
        public Task Edit(Produto entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.ProdutoSet.Update(entity));
        }
        public async Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .Where(p => p.VendedorId == vendedorId)
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Produto>> GetAllProdutoWithCategoria( CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .ToListAsync(cancellationToken);
        }
        public async Task<Produto> GetSelfWithCategoriaById(Guid id, Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id && p.VendedorId == vendedorId, cancellationToken);
        }

        public async Task<Produto> GetWithCategoriaById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id , cancellationToken);
        }
        public async Task<Produto> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<Produto> GetSelfProdutoById(Guid id, Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .FirstOrDefaultAsync(c => c.Id == id && c.VendedorId == vendedorId, cancellationToken);
        }

        public async Task<List<Produto>> ListWithCategoriaVendedorAsNoTracking(CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.Ativo == true && p.Vendedor.Ativo == true)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Produto>> ListWithCategoriaVendedorByCategoriaAsNoTracking(Guid categoriaId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.CategoriaId == categoriaId && p.Ativo == true && p.Vendedor.Ativo == true)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Produto>> ListWithCategoriaVendedorByVendedorAsNoTracking(Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .Where(p => p.VendedorId == vendedorId && p.Ativo && p.Vendedor.Ativo)
                .ToListAsync(cancellationToken);
        }

        public async Task<Produto> getProdutoWithCategoriaVendedorById(Guid produtoId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == produtoId && p.Ativo == true && p.Vendedor.Ativo == true, cancellationToken);
        }

        public async Task Remove(Produto produto, CancellationToken cancellationToken)
        {
            Task.FromResult(_context.ProdutoSet.Remove(produto));
        }
        public async Task<bool> Exists(string nome, CancellationToken cancellationToken)
        {
            return await _context.ProdutoSet.AnyAsync(c => c.Nome == nome, cancellationToken);
        }

        public async Task<List<Produto>> List(Guid vendedorId, CancellationToken cancellationToken)
        {
            return await _context
                .ProdutoSet
                .AsNoTracking()
                .Where(p => p.VendedorId == vendedorId)
                .ToListAsync(cancellationToken);            
        }
        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<Produto>> ListWithCategoriaVendedorPagedAsNoTracking(int pagina, int tamanho, CancellationToken cancellationToken)
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

        public async Task<PagedResult<Produto>> ListWithCategoriaVendedorByCategoriaPagedAsNoTracking(Guid categoriaId, int pagina, int tamanho, CancellationToken cancellationToken)
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

        public async Task<PagedResult<Produto>> ListWithCategoriaVendedorByVendedorPagedAsNoTracking(Guid vendedorId, int pagina, int tamanho, CancellationToken cancellationToken)
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
