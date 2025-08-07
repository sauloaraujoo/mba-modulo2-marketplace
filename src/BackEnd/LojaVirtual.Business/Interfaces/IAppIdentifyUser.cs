namespace LojaVirtual.Business.Interfaces
{
    public interface IAppIdentifyUser
    {
        public string ObterUsuarioId();
        bool EAutenticado();
    }
}