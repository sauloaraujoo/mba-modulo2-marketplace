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

        public async Task Inserir(Categoria categoria, CancellationToken tokenDeCancelamento)
        {
            //verifica se o id da categoria já existe
            if (await _categoriaRepository.ObterPorId(categoria.Id, tokenDeCancelamento) is not null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Id da categoria já existente"));
                return;
            }

            //verifica se o nome da categoria já existe
            if (await _categoriaRepository.Existe(categoria.Nome, tokenDeCancelamento))
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Nome da categoria já existente"));
                return;
            }
            await _categoriaRepository.Inserir(categoria, tokenDeCancelamento);
            await _categoriaRepository.SalvarMudancas(tokenDeCancelamento);
        }

        public async Task Editar(Categoria categoria, CancellationToken tokenDeCancelamento)
        {
            var categoriaOrigem = await _categoriaRepository.ObterPorId(categoria.Id, tokenDeCancelamento);
            if(categoriaOrigem is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
                return;
            }
            if (categoriaOrigem.Nome != categoria.Nome &&
                await _categoriaRepository.Existe(categoria.Nome, tokenDeCancelamento))
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Nome da categoria já existente."));
                return;
            }

            categoriaOrigem.Edit(categoria.Nome, categoria.Descricao);

            await _categoriaRepository.Editar(categoriaOrigem, tokenDeCancelamento);
            await _categoriaRepository.SalvarMudancas(tokenDeCancelamento);
        }

        public async Task Remover(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoria = await _categoriaRepository.ObterComProduto(id, tokenDeCancelamento);
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

            await _categoriaRepository.Remover(categoria, tokenDeCancelamento);
            await _categoriaRepository.SalvarMudancas(tokenDeCancelamento);
        }        

        public async Task<IEnumerable<Categoria>> Listar(CancellationToken tokenDeCancelamento)
        {
            return await _categoriaRepository.ListarSemContexto(tokenDeCancelamento);
        }

        public async Task<Categoria> ObterPorId(Guid id, CancellationToken tokenDeCancelamento)
        {
            var categoria = await _categoriaRepository.ObterPorId(id, tokenDeCancelamento);
            if (categoria is null)
            {
                _notificavel.AdicionarNotificacao(new Notificacao("Categoria não encontrada."));
            }
            return categoria!;
        }
    }
}
