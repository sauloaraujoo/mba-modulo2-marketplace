using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaRepository : IDisposable
    {
        public Task Inserir(Categoria categoria, CancellationToken tokenDeCancelamento);
        public Task<Categoria> ObterPorId(Guid id, CancellationToken tokenDeCancelamento);
        public Task<IList<Categoria>> ListarSemContexto(CancellationToken tokenDeCancelamento);
        public Task Editar(Categoria categoria, CancellationToken tokenDeCancelamento);
        public Task Remover(Categoria categoria, CancellationToken tokenDeCancelamento);
        public Task<Categoria> ObterComProduto(Guid id, CancellationToken tokenDeCancelamento);
        public Task<bool> Existe(string nome, CancellationToken tokenDeCancelamento);
        public Task<int> SalvarMudancas(CancellationToken tokenDeCancelamento);
    }
}
