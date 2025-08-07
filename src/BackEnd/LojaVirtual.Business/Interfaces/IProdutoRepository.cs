using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoRepository : IDisposable
    {
        public Task Inserir(Produto produto, CancellationToken cancellationToken);
        public Task<Produto> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<Produto> ObterProdutoProprioPorId(Guid id, Guid vendedorId, CancellationToken cancellationToken);
        public Task<List<Produto>> ListarComCategoriaVendedorSemContexto(CancellationToken cancellationToken);
        public Task<PagedResult<Produto>> ListarComCategoriaVendedorPaginadoSemContexto(int pagina, int tamanho, CancellationToken cancellationToken);
        public Task<PagedResult<Produto>> ListarComCategoriaVendedorPorCategoriaPaginadoSemContexto(Guid categoriaId, int pagina, int tamanho, CancellationToken cancellationToken);
        public Task<PagedResult<Produto>> ListarComCategoriaVendedorPorVendedorPaginadoSemContexto(Guid vendedorId, int pagina, int tamanho, CancellationToken cancellationToken);
        public Task<Produto> ObterProdutoComCategoriaVendedorPorId(Guid produtoId, CancellationToken cancellationToken);
        public Task<List<Produto>> ListarComCategoriaVendedorPorCategoriaSemContexto(Guid categoriaId, CancellationToken cancellationToken);
        public Task<List<Produto>> ListarComCategoriaVendedorPorVendedorSemContexto(Guid vendedorId, CancellationToken cancellationToken);
        public Task<List<Produto>> Listar(Guid vendedorId, CancellationToken cancellationToken);
        public Task Editar(Produto produto, CancellationToken cancellationToken);
        public Task Remover(Produto produto, CancellationToken cancellationToken);
        public Task<bool> Existe(string nome, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterTodosProdutosPropriosComCategoria(Guid vendedorid, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterTodosProdutosComCategoria(CancellationToken cancellationToken);
        Task<Produto> ObterProprioComCategoriaPorId(Guid id, Guid vendedorid, CancellationToken cancellationToken);
        Task<Produto> ObterComCategoriaPorId(Guid id, CancellationToken cancellationToken);
        public Task<int> SalvarMudancas(CancellationToken cancellationToken);
    }
}
