﻿using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoService
    {
        Task Insert(Produto request, CancellationToken cancellationToken);
        Task Edit(Produto request, CancellationToken cancellationToken);
        Task Remove(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> Lista(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ListVitrine(Guid? categoriaId, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ListVitrineByVendedor(Guid? vendedorId, CancellationToken cancellationToken);

        Task<PagedResult<Produto>> ListarVitrinePaginado(Guid? categoriaId, int pagina, int tamanho, CancellationToken cancellationToken);
        Task<PagedResult<Produto>> ListarVitrinePorVendedorPaginado(Guid? vendedorId, int pagina, int tamanho, CancellationToken cancellationToken);

        Task<Produto> ListarVitrinePorId(Guid? produtoId, CancellationToken cancellationToken);

        Task<Produto> GetById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> GetAllSelfProdutoWithCategoria(CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> GetAllProdutoWithCategoria(CancellationToken cancellationToken);
        Task<Produto> GetSelfWithCategoriaById(Guid id, CancellationToken cancellationToken);
        Task<Produto> GetWithCategoriaById(Guid id, CancellationToken cancellationToken);

        Task<Produto?> ObterProdutoPorId(Guid id, CancellationToken cancellationToken);

        Task AlterarStatus(Produto request, CancellationToken cancellationToken);
    }
}
