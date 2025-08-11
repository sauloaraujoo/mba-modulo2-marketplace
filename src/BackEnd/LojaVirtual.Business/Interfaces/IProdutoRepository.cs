using LojaVirtual.Business.Common;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IProdutoRepository : IDisposable
    {
        public Task Inserir(Produto produto, CancellationToken tokenDeCancelamento);
        public Task<Produto> ObterPorId(Guid id, CancellationToken tokenDeCancelamento);
        Task<Produto> ObterProdutoProprioPorId(Guid id, Guid vendedorId, CancellationToken tokenDeCancelamento);
        public Task<List<Produto>> ListarComCategoriaVendedorSemContexto(CancellationToken tokenDeCancelamento);
        public Task<PagedResult<Produto>> ListarComCategoriaVendedorPaginadoSemContexto(int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        public Task<PagedResult<Produto>> ListarComCategoriaVendedorPorCategoriaPaginadoSemContexto(Guid categoriaId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        public Task<PagedResult<Produto>> ListarComCategoriaVendedorPorVendedorPaginadoSemContexto(Guid vendedorId, int pagina, int tamanho, CancellationToken tokenDeCancelamento);
        public Task<Produto> ObterProdutoComCategoriaVendedorPorId(Guid produtoId, CancellationToken tokenDeCancelamento);
        public Task<List<Produto>> ListarComCategoriaVendedorPorCategoriaSemContexto(Guid categoriaId, CancellationToken tokenDeCancelamento);
        public Task<List<Produto>> ListarComCategoriaVendedorPorVendedorSemContexto(Guid vendedorId, CancellationToken tokenDeCancelamento);
        public Task<List<Produto>> Listar(Guid vendedorId, CancellationToken tokenDeCancelamento);
        public Task Editar(Produto produto, CancellationToken tokenDeCancelamento);
        public Task Remover(Produto produto, CancellationToken tokenDeCancelamento);
        public Task<bool> Existe(string nome, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ObterTodosProdutosPropriosComCategoriaVendedor(Guid vendedorid, CancellationToken tokenDeCancelamento);
        Task<IEnumerable<Produto>> ObterTodosProdutosComCategoriaVendedor(CancellationToken tokenDeCancelamento);
        Task<Produto> ObterProprioComCategoriaVendedorPorId(Guid id, Guid vendedorid, CancellationToken tokenDeCancelamento);
        Task<Produto> ObterComCategoriaVendedorPorId(Guid id, CancellationToken tokenDeCancelamento);
        public Task<int> SalvarMudancas(CancellationToken tokenDeCancelamento);
    }
}
