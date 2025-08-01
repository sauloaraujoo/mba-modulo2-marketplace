﻿using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoRepository : IDisposable
    {
        public Task Insert(Produto produto, CancellationToken cancellationToken);
        public Task<Produto> GetById(Guid id, CancellationToken cancellationToken);
        Task<Produto> GetSelfProdutoById(Guid id, Guid vendedorId, CancellationToken cancellationToken);
        public Task<List<Produto>> ListWithCategoriaVendedorAsNoTracking(CancellationToken cancellationToken);
        public Task<PagedResult<Produto>> ListWithCategoriaVendedorPagedAsNoTracking(int pagina, int tamanho, CancellationToken cancellationToken);
        public Task<PagedResult<Produto>> ListWithCategoriaVendedorByCategoriaPagedAsNoTracking(Guid categoriaId, int pagina, int tamanho, CancellationToken cancellationToken);
        public Task<PagedResult<Produto>> ListWithCategoriaVendedorByVendedorPagedAsNoTracking(Guid vendedorId, int pagina, int tamanho, CancellationToken cancellationToken);
        public Task<Produto> getProdutoWithCategoriaVendedorById(Guid produtoId, CancellationToken cancellationToken);

        public Task<List<Produto>> ListWithCategoriaVendedorByCategoriaAsNoTracking(Guid categoriaId, CancellationToken cancellationToken);
        public Task<List<Produto>> ListWithCategoriaVendedorByVendedorAsNoTracking(Guid vendedorId, CancellationToken cancellationToken);
        public Task<List<Produto>> List(Guid vendedorId, CancellationToken cancellationToken);
        public Task Edit(Produto produto, CancellationToken cancellationToken);
        public Task Remove(Produto produto, CancellationToken cancellationToken);
        public Task<bool> Exists(string nome, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(Guid vendedorid, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> GetAllProdutoWithCategoria(CancellationToken cancellationToken);
        Task<Produto> GetSelfWithCategoriaById(Guid id, Guid vendedorid, CancellationToken cancellationToken);
        Task<Produto> GetWithCategoriaById(Guid id, CancellationToken cancellationToken);
        public Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
