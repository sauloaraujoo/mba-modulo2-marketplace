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

        public async Task Inserir(Categoria categoria, CancellationToken cancellationToken)
        {
            //verifica se o id da categoria já existe
            if (await _categoriaRepository.ObterPorId(categoria.Id, cancellationToken) is not null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Id da categoria já existente"));
                return;
            }

            //verifica se o nome da categoria já existe
            if (await _categoriaRepository.Existe(categoria.Nome, cancellationToken))
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Nome da categoria já existente"));
                return;
            }
            await _categoriaRepository.Inserir(categoria, cancellationToken);
            await _categoriaRepository.SalvarMudancas(cancellationToken);
        }

        public async Task Editar(Categoria categoria, CancellationToken cancellationToken)
        {
            var categoriaOrigem = await _categoriaRepository.ObterPorId(categoria.Id, cancellationToken);
            if(categoriaOrigem is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
                return;
            }
            if (categoriaOrigem.Nome != categoria.Nome &&
                await _categoriaRepository.Existe(categoria.Nome, cancellationToken))
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Nome da categoria já existente."));
                return;
            }

            categoriaOrigem.Edit(categoria.Nome, categoria.Descricao);

            await _categoriaRepository.Editar(categoriaOrigem, cancellationToken);
            await _categoriaRepository.SalvarMudancas(cancellationToken);
        }

        public async Task Remover(Guid id, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterComProduto(id, cancellationToken);
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

            await _categoriaRepository.Remover(categoria, cancellationToken);
            await _categoriaRepository.SalvarMudancas(cancellationToken);
        }        

        public async Task<IEnumerable<Categoria>> Listar(CancellationToken cancellationToken)
        {
            return await _categoriaRepository.ListarSemContexto(cancellationToken);
        }

        public async Task<Categoria> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorId(id, cancellationToken);
            if (categoria is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
            }
            return categoria!;
        }
    }
}
