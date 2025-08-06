using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notifications;

namespace LojaVirtual.Business.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly INotifiable _notifiable;
        public CategoriaService(
            ICategoriaRepository categoriaRepository, 
            INotifiable notifiable)
        {
            _categoriaRepository = categoriaRepository;
            _notifiable = notifiable;
        }

        public async Task Inserir(Categoria categoria, CancellationToken cancellationToken)
        {
            //verifica se o id da categoria já existe
            if (await _categoriaRepository.ObterPorId(categoria.Id, cancellationToken) is not null)
            {
                _notifiable.AddNotification(new Notification("Id da categoria já existente"));
                return;
            }

            //verifica se o nome da categoria já existe
            if (await _categoriaRepository.Exists(categoria.Nome, cancellationToken))
            {
                _notifiable.AddNotification(new Notification("Nome da categoria já existente"));
                return;
            }
            await _categoriaRepository.Insert(categoria, cancellationToken);
            await _categoriaRepository.SaveChanges(cancellationToken);
        }

        public async Task Editar(Categoria categoria, CancellationToken cancellationToken)
        {
            var categoriaOrigem = await _categoriaRepository.ObterPorId(categoria.Id, cancellationToken);
            if(categoriaOrigem is null)
            {
                _notifiable.AddNotification(new Notification("Categoria não encontrada."));
                return;
            }
            if (categoriaOrigem.Nome != categoria.Nome &&
                await _categoriaRepository.Exists(categoria.Nome, cancellationToken))
            {
                _notifiable.AddNotification(new Notification("Nome da categoria já existente."));
                return;
            }

            categoriaOrigem.Edit(categoria.Nome, categoria.Descricao);

            await _categoriaRepository.Edit(categoriaOrigem, cancellationToken);
            await _categoriaRepository.SaveChanges(cancellationToken);
        }

        public async Task Remove(Guid id, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterComProduto(id, cancellationToken);
            if (categoria is null)
            {
                _notifiable.AddNotification(new Notification("Categoria não encontrada."));
                return;
            }
            if (categoria.Produtos.Any())
            {
                _notifiable.AddNotification(new Notification("Categoria possui produtos associados."));
                return;
            }

            await _categoriaRepository.Remove(categoria, cancellationToken);
            await _categoriaRepository.SaveChanges(cancellationToken);
        }        

        public async Task<IEnumerable<Categoria>> List(CancellationToken cancellationToken)
        {
            return await _categoriaRepository.ListAsNoTracking(cancellationToken);
        }

        public async Task<Categoria> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorId(id, cancellationToken);
            if (categoria is null)
            {
                _notifiable.AddNotification(new Notification("Categoria não encontrada."));
            }
            return categoria!;
        }
    }
}
