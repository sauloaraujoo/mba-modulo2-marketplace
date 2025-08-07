using LojaVirtual.Business.Notificacoes;

namespace LojaVirtual.Business.Interfaces
{
    public interface INotificavel
    {
        bool Valido();
        bool Invalido();
        List<Notificacao> ObterNotificacoes();
        void AdicionarNotificacao(Notificacao notificacao);
    }
}
