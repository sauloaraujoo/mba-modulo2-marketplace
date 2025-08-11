using LojaVirtual.Business.Entities;

namespace LojaVirtual.Business.Interfaces
{
    public interface IClienteRepository : IDisposable
    {
        Task Inserir(Cliente cliente, CancellationToken tokenDeCancelamento);
        Task<Cliente?> ObterClienteComFavoritos(Guid id, CancellationToken tokenDeCancelamento);
        public Task<int> SalvarMudancas(CancellationToken tokenDeCancelamento);
        void Editar(Cliente cliente);
    }
}
