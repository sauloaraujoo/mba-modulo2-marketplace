using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Notificacoes;

namespace LojaVirtual.Business.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly INotificavel _notificavel;
        public CategoriaService(
            ICategoriaRepository categoriaRepository, 
            INotificavel notificavel)
        {
            _categoriaRepository = categoriaRepository;
            _notificavel = notificavel;
        }

        public async Task Insert(Categoria categoria, CancellationToken tokenDeCancelamento)
        {
            //verifica se o id da categoria já existe
            if (await _categoriaRepository.GetById(categoria.Id, tokenDeCancelamento) is not null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Id da categoria já existente"));
                return;
            }

            //verifica se o nome da categoria já existe
            if (await _categoriaRepository.Exists(categoria.Nome, tokenDeCancelamento))
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Nome da categoria já existente"));
                return;
            }
            await _categoriaRepository.Insert(categoria, tokenDeCancelamento);
            await _categoriaRepository.SaveChanges(tokenDeCancelamento);
        }

        public async Task Edit(Categoria categoria, CancellationToken tokenDeCancelamento)
        {
            var categoriaOrigem = await _categoriaRepository.GetById(categoria.Id, tokenDeCancelamento);
            if(categoriaOrigem is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
                return;
            }
            if (categoriaOrigem.Nome != categoria.Nome &&
                await _categoriaRepository.Exists(categoria.Nome, tokenDeCancelamento))
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Nome da categoria já existente."));
                return;
            }

            categoriaOrigem.Edit(categoria.Nome, categoria.Descricao);

            await _categoriaRepository.Edit(categoriaOrigem, tokenDeCancelamento);
            await _categoriaRepository.SaveChanges(tokenDeCancelamento);
        }

        public async Task Remove(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoria = await _categoriaRepository.GetWithProduto(id, tokenDeCancelamento);
            if (categoria is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
                return;
            }
            if (categoria.Produtos.Any())
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria possui produtos associados."));
                return;
            }

            await _categoriaRepository.Remove(categoria, tokenDeCancelamento);
            await _categoriaRepository.SaveChanges(tokenDeCancelamento);
        }        

        public async Task<IEnumerable<Categoria>> List(CancellationToken tokenDeCancelamento)
        {
            return await _categoriaRepository.ListAsNoTracking(tokenDeCancelamento);
        }

        public async Task<Categoria> GetById(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoria = await _categoriaRepository.GetById(id, tokenDeCancelamento);
            if (categoria is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
            }
            return categoria!;
        }
    }
}
