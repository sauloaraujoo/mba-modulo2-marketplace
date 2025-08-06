using LojaVirtual.Business.Interfaces;

namespace LojaVirtual.Business.Notificacoes
{
    public class Notificavel : INotificavel
    {
        private readonly List<Notificacao> _notificacoes;
        public Notificavel()
        {
            _notificacoes = new List<Notificacao>();
        }
        public IReadOnlyCollection<Notificacao> Notificacoes { get { return _notificacoes; } }

        public void AdicionarNotificacao(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }
        public List<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }
        public bool Invalido()
        {
            return _notificacoes.Any();
        }
        public bool Valido()
        {
            return !Invalido();
        }
    }
}
