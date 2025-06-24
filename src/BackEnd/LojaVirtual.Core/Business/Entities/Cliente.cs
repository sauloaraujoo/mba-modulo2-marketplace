namespace LojaVirtual.Core.Business.Entities
{
    public class Cliente : Entity
    {
        protected Cliente() { _favoritos = new List<Favorito>(); }
        public Cliente(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
            _favoritos = new List<Favorito>();
        }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        private readonly List<Favorito> _favoritos;
        public IReadOnlyCollection<Favorito> Favoritos => _favoritos;
        public void AddFavorito(Guid produtoId)
        {
            var favorito = new Favorito(Id, produtoId);
            _favoritos.Add(favorito);
        }
        public void RemoveFavorito(Favorito favorito)
        {
            _favoritos.Remove(favorito);
        }
    }
}
