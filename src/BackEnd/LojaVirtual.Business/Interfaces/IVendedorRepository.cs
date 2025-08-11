using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IVendedorRepository : IDisposable
    {
        Task Inserir(Vendedor request, CancellationToken tokenDeCancelamento);
        Task<Vendedor> ObterPorIdSemContexto(Guid id, CancellationToken tokenDeCancelamento);
        public Task<IList<Vendedor>> ListarSemContexto(CancellationToken tokenDeCancelamento);
        public Task Editar(Vendedor vendedor, CancellationToken tokenDeCancelamento);
        public Task<bool> Existe(string nome, CancellationToken tokenDeCancelamento);
        public Task<int> SalvarMudancas(CancellationToken tokenDeCancelamento);
    }
}
