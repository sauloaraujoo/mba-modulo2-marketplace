namespace LojaVirtual.Core.Business.Interfaces
{
    public interface IAppIdentifyUser
    {
        public string GetUserId();
        bool IsAuthenticated();        
    }
}
