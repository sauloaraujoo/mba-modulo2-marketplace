using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface ICategoriaRepository : IDisposable
    {
        public Task Inserir(Categoria categoria, CancellationToken cancellationToken);
        public Task<Categoria> ObterPorId(Guid id, CancellationToken cancellationToken);
        public Task<IList<Categoria>> ListarSemContexto(CancellationToken cancellationToken);
        public Task Editar(Categoria categoria, CancellationToken cancellationToken);
        public Task Remover(Categoria categoria, CancellationToken cancellationToken);
        public Task<Categoria> ObterComProduto(Guid id, CancellationToken cancellationToken);
        public Task<bool> Existe(string nome, CancellationToken cancellationToken);
        public Task<int> SalvarMudancas(CancellationToken cancellationToken);
    }
}
