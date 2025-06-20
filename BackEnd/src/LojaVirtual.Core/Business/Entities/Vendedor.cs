namespace LojaVirtual.Core.Business.Entities
{
    public class Vendedor: Entity
    {
        public Vendedor(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }
        public string Nome { get; private set; }
        public string Email { get; private set; }
    }
}
