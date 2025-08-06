﻿using LojaVirtual.Business.Entities;
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

        public async Task Insert(Categoria entity, CancellationToken cancellationToken)
        {
            await _context.CategoriaSet.AddAsync(entity, cancellationToken);
        }
        public Task Edit(Categoria entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.CategoriaSet.Update(entity));
        }

        public async Task<Categoria> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _context.CategoriaSet.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Categoria> ObterComProduto(Guid id, CancellationToken cancellationToken)
        {
            return await _context
                            .CategoriaSet
                            .Include(c => c.Produtos)
                            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<IList<Categoria>> ListAsNoTracking(CancellationToken cancellationToken)
        {
            return await _context
                            .CategoriaSet
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);
        }
        public async Task Remove(Categoria categoria, CancellationToken cancellationToken)
        {
            Task.FromResult(_context.CategoriaSet.Remove(categoria));
        }
        public async Task<bool> Exists(string nome, CancellationToken cancellationToken)
        {
            return await _context.CategoriaSet.AnyAsync(c => c.Nome == nome, cancellationToken);
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
