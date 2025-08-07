using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notificacoes;

namespace LojaVirtual.Business.Services
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;
        private readonly INotificavel _notificavel;

        public VendedorService(
             IVendedorRepository vendedorRepository,
             INotificavel notificavel)
        {
            _vendedorRepository = vendedorRepository;
            _notificavel = notificavel;
        }

      
        public async Task AlterarStatus(Vendedor vendedor, CancellationToken tokenDeCancelamento)
        {
            var vendedorOrigem = await _vendedorRepository.GetById(vendedor.Id, tokenDeCancelamento);
            if (vendedorOrigem is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Vendedor não encontrado."));
                return;
            }

            vendedorOrigem.AlterarStatus();

            await _vendedorRepository.Edit(vendedorOrigem, tokenDeCancelamento);
            await _vendedorRepository.SaveChanges(tokenDeCancelamento);
        }

        public async Task<IEnumerable<Vendedor>> Listar(CancellationToken tokenDeCancelamento)
        {
            return await _vendedorRepository.ListAsNoTracking(tokenDeCancelamento);
        }

        public async Task<Vendedor> ObterPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            var vendedor = await _vendedorRepository.GetById(id, tokenDeCancelamento);
            if (vendedor is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Vendedor não encontrado."));
            }
            return vendedor!;
        }

    }
}
