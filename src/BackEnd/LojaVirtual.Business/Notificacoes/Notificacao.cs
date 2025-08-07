namespace LojaVirtual.Business.Notificacoes
{
    public class Notificacao
    {
        public Notificacao(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
