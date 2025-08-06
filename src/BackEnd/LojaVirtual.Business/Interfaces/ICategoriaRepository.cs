﻿using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaRepository : IDisposable
    {
        public Task Insert(Categoria categoria, CancellationToken cancellationToken);
        public Task<Categoria> ObterPorId(Guid id, CancellationToken cancellationToken);
        public Task<IList<Categoria>> ListAsNoTracking(CancellationToken cancellationToken);
        public Task Edit(Categoria categoria, CancellationToken cancellationToken);
        public Task Remove(Categoria categoria, CancellationToken cancellationToken);
        public Task<Categoria> ObterComProduto(Guid id, CancellationToken cancellationToken);
        public Task<bool> Exists(string nome, CancellationToken cancellationToken);
        public Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
