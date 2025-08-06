using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notifications;

namespace LojaVirtual.Business.Services
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;
        private readonly INotifiable _notifiable;

        public VendedorService(
             IVendedorRepository vendedorRepository,
             INotifiable notifiable)
        {
            _vendedorRepository = vendedorRepository;
            _notifiable = notifiable;
        }

      
        public async Task AlterarStatus(Vendedor vendedor, CancellationToken cancellationToken)
        {
            var vendedorOrigem = await _vendedorRepository.GetById(vendedor.Id, cancellationToken);
            if (vendedorOrigem is null)
            {
                _notifiable.AddNotification(new Notification("Vendedor não encontrado."));
                return;
            }

            vendedorOrigem.AlterarStatus();

            await _vendedorRepository.Edit(vendedorOrigem, cancellationToken);
            await _vendedorRepository.SaveChanges(cancellationToken);
        }

        public async Task<IEnumerable<Vendedor>> List(CancellationToken cancellationToken)
        {
            return await _vendedorRepository.ListAsNoTracking(cancellationToken);
        }

        public async Task<Vendedor> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            var vendedor = await _vendedorRepository.GetById(id, cancellationToken);
            if (vendedor is null)
            {
                _notifiable.AddNotification(new Notification("Vendedor não encontrado."));
            }
            return vendedor!;
        }

    }
}
